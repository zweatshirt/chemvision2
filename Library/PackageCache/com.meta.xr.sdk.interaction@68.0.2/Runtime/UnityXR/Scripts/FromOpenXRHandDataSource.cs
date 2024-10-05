﻿/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Oculus.Interaction.GrabAPI;
using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace Oculus.Interaction.Input.UnityXR
{
    /// <summary>
    ///   <para>Accepts OpenXR Hand Data returning an ISDK compatible HandDataAsset.</para>
    /// </summary>
    public abstract class FromOpenXRHandDataSource : DataSource<HandDataAsset>
    {
        [Serializable]
        public class OpenXRHandDataAsset : ICopyFrom<OpenXRHandDataAsset>
        {
            public static class Constants
            {
                public const int NUM_HAND_JOINTS = 26;
                public const int NUM_FINGERS = 5;
            }

            [Flags]
            public enum JointTrackingState
            {
                None = 0,
                Radius = 1,
                Pose = 2,
                LinearVelocity = 4,
                AngularVelocity = 8,
                WillNeverBeValid = 16
            }

            [Flags]
            [Preserve]
            public enum AimFlagsFB : ulong
            {
                None = 0,
                Computed = 1,
                Valid = 2,
                IndexPinching = 4,
                MiddlePinching = 8,
                RingPinching = 16, // 0x0000000000000010
                LittlePinching = 32, // 0x0000000000000020
                SystemGesture = 64, // 0x0000000000000040
                DominantHand = 128, // 0x0000000000000080
                MenuPressed = 256, // 0x0000000000000100
            }

            public bool IsDataValid;
            public bool IsConnected;
            public bool IsTracked;

            // XR_EXT_hand_tracking
            public Pose Root;
            public PoseOrigin RootPoseOrigin;
            public JointTrackingState[] JointStates = new JointTrackingState[Constants.NUM_HAND_JOINTS];
            public Pose[] JointPoses = new Pose[Constants.NUM_HAND_JOINTS];
            public float[] JointRadiuses = new float[Constants.NUM_HAND_JOINTS];
            public Vector3[] JointAngularVelocities = new Vector3[Constants.NUM_HAND_JOINTS];
            public Vector3[] JointLinearVelocities = new Vector3[Constants.NUM_HAND_JOINTS];

            // XR_FB_hand_tracking_aim
            public AimFlagsFB AimFlags;
            public float[] FingerPinchStrength = new float[Constants.NUM_FINGERS];
            public Pose PointerPose;
            public PoseOrigin PointerPoseOrigin;

            public HandDataSourceConfig Config = new();
            public void CopyFrom(OpenXRHandDataAsset source)
            {
                IsDataValid = source.IsDataValid;
                IsConnected = source.IsConnected;
                IsTracked = source.IsTracked;
                AimFlags = source.AimFlags;
                Config = source.Config;
                CopyPosesFrom(source);
            }

            private void CopyPosesFrom(OpenXRHandDataAsset source)
            {
                Root = source.Root;
                RootPoseOrigin = source.RootPoseOrigin;
                Array.Copy(source.JointStates, JointStates, Constants.NUM_HAND_JOINTS);
                Array.Copy(source.JointPoses, JointPoses, Constants.NUM_HAND_JOINTS);
                Array.Copy(source.JointRadiuses, JointRadiuses, Constants.NUM_HAND_JOINTS);
                Array.Copy(source.JointAngularVelocities, JointAngularVelocities, Constants.NUM_HAND_JOINTS);
                Array.Copy(source.JointLinearVelocities, JointLinearVelocities, Constants.NUM_HAND_JOINTS);

                Array.Copy(source.FingerPinchStrength, FingerPinchStrength, FingerPinchStrength.Length);
                PointerPose = source.PointerPose;
                PointerPoseOrigin = source.PointerPoseOrigin;
                Config = source.Config;
            }
        }

        private const float PressThreshold = 0.8f;
        static readonly Vector3 TrackedRemoteAimOffset = new(0.0f, 0.0f, -0.055f);

        [SerializeField, Interface(typeof(IHmd))]
        private UnityEngine.Object _hmdData;
        private IHmd HmdData;

        private readonly HandDataAsset _dataAsset = new();

        // Meta Hand Aim Mocking
        private HandJointCache _jointCache;
        private FingerPinchGrabAPI _fingerGrabAPI;

        protected abstract OpenXRHandDataAsset OpenXRData { get; }

        protected virtual void Awake()
        {
            HmdData = _hmdData as IHmd;
        }

        protected override void Start()
        {
            this.BeginStart(ref _started, () => base.Start());
            this.AssertField(HmdData, nameof(HmdData));
            this.EndStart(ref _started);
        }

        protected override void UpdateData()
        {
            var openXRData = OpenXRData;
            _dataAsset.CopyFrom(openXRData);

            // if XR_FB_hand_tracking_aim is unavailable
            if (_dataAsset.IsDataValidAndConnected && openXRData.AimFlags == OpenXRHandDataAsset.AimFlagsFB.None)
            {
                PopulateMockHandTrackingAim(openXRData.JointPoses[0]);
            }
        }


        private void PopulateMockHandTrackingAim(Pose xrPalmPose)
        {
            // Update Joint Cache
            _jointCache ??= new HandJointCache(_dataAsset.Config.HandSkeleton);
            _jointCache.Update(_dataAsset, CurrentDataVersion);
            _jointCache.GetAllPosesFromWrist(out var localJointPoses);

            _fingerGrabAPI ??= new FingerPinchGrabAPI(HmdData);
            _fingerGrabAPI.Update(localJointPoses, _dataAsset.Config.Handedness, _dataAsset.Root);
            PopulateMockHandTrackingAimFinger(HandFinger.Index);
            PopulateMockHandTrackingAimFinger(HandFinger.Middle);
            PopulateMockHandTrackingAimFinger(HandFinger.Ring);
            PopulateMockHandTrackingAimFinger(HandFinger.Pinky);


            _dataAsset.PointerPose =
                xrPalmPose.GetTransformedBy(new Pose(TrackedRemoteAimOffset, Quaternion.identity));
            _dataAsset.PointerPoseOrigin = PoseOrigin.SyntheticPose;
            _dataAsset.IsDominantHand = _dataAsset.Config.Handedness == Handedness.Right;
        }

        private void PopulateMockHandTrackingAimFinger(HandFinger finger)
        {
            var fingerIndex = (int)finger;
            _dataAsset.FingerPinchStrength[fingerIndex] =
                _fingerGrabAPI.GetFingerGrabScore(finger);
            _dataAsset.IsFingerPinching[fingerIndex] =
                _dataAsset.FingerPinchStrength[fingerIndex] > PressThreshold;
        }

        protected override HandDataAsset DataAsset => _dataAsset;

    }
}

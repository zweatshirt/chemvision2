// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

#pragma warning disable 0618

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Oculus.Platform.Models;
  using System.Collections.Generic;
  using UnityEngine;

  /// The leaderboard object contains information about this leaderboard ID, API
  /// name and destination.
  public class Leaderboard
  {
    /// The API name of this leaderboard. This is a unique string that your
    /// application will refer to this leaderboard in your app code.
    public readonly string ApiName;
    /// An optional Deep Link Destination, which means when a user clicks on the
    /// leaderboard, they will be taken to this in-app destination.
    // May be null. Check before using.
    public readonly Destination DestinationOptional;
    [Obsolete("Deprecated in favor of DestinationOptional")]
    public readonly Destination Destination;
    /// The generated GUID of this leaderboard.
    public readonly UInt64 ID;


    public Leaderboard(IntPtr o)
    {
      ApiName = CAPI.ovr_Leaderboard_GetApiName(o);
      {
        var pointer = CAPI.ovr_Leaderboard_GetDestination(o);
        Destination = new Destination(pointer);
        if (pointer == IntPtr.Zero) {
          DestinationOptional = null;
        } else {
          DestinationOptional = Destination;
        }
      }
      ID = CAPI.ovr_Leaderboard_GetID(o);
    }
  }

  public class LeaderboardList : DeserializableList<Leaderboard> {
    public LeaderboardList(IntPtr a) {
      var count = (int)CAPI.ovr_LeaderboardArray_GetSize(a);
      _Data = new List<Leaderboard>(count);
      for (int i = 0; i < count; i++) {
        _Data.Add(new Leaderboard(CAPI.ovr_LeaderboardArray_GetElement(a, (UIntPtr)i)));
      }

      _NextUrl = CAPI.ovr_LeaderboardArray_GetNextUrl(a);
    }

  }
}

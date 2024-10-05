// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform
{

  using Description = System.ComponentModel.DescriptionAttribute;

  public enum PlatformInitializeResult : int
  {
    /// Oculus Platform SDK initialization succeeded.
    [Description("SUCCESS")]
    Success = 0,

    /// Oculus Platform SDK was not initialized.
    [Description("UNINITIALIZED")]
    Uninitialized = -1,

    /// Oculus Platform SDK failed to initialize because the pre-loaded module was
    /// on a different path than the validated library.
    [Description("PRE_LOADED")]
    PreLoaded = -2,

    /// Oculus Platform SDK files failed to load.
    [Description("FILE_INVALID")]
    FileInvalid = -3,

    /// Oculus Platform SDK failed to initialize due to an invalid signature in the
    /// signed certificate.
    [Description("SIGNATURE_INVALID")]
    SignatureInvalid = -4,

    /// Oculus Platform SDK failed to verify the application's signature during
    /// initialization
    [Description("UNABLE_TO_VERIFY")]
    UnableToVerify = -5,

    /// There was a mismatch between the version of Oculus Platform SDK used by the
    /// application and the version installed on the Oculus user's device.
    [Description("VERSION_MISMATCH")]
    VersionMismatch = -6,

    [Description("UNKNOWN")]
    Unknown = -7,

    /// Oculus Platform SDK failed to initialize because the Oculus user had an
    /// invalid account access token.
    [Description("INVALID_CREDENTIALS")]
    InvalidCredentials = -8,

    /// Oculus Platform SDK failed to initialize because the Oculus user does not
    /// have the application entitlement.
    [Description("NOT_ENTITLED")]
    NotEntitled = -9,

  }

}

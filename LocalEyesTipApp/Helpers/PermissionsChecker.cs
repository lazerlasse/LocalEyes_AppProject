using System.Security.Permissions;

namespace LocalEyesTipApp.Helpers
{
    public static class PermissionsChecker
    {
        public static async Task<bool> CheckPermissions()
        {
#if ANDROID
            PermissionStatus mediaStatus = await CheckPermissions<Permissions.Media>();
            PermissionStatus cameraStatus = await CheckPermissions<Permissions.Camera>();
            PermissionStatus photosStatus = await CheckPermissions<Permissions.Photos>();
            PermissionStatus internetStatus = await CheckPermissions<Permissions.NetworkState>();
            return IsGranted(cameraStatus) && IsGranted(mediaStatus) && IsGranted(photosStatus) && IsGranted(internetStatus);
#else
            PermissionStatus storageReadStatus = await CheckPermissions<Permissions.StorageRead>();
            PermissionStatus storageWriteStatus = await CheckPermissions<Permissions.StorageWrite>();
            PermissionStatus cameraStatus = await CheckPermissions<Permissions.Camera>();
            PermissionStatus photosStatus = await CheckPermissions<Permissions.Photos>();
            PermissionStatus internetStatus = await CheckPermissions<Permissions.NetworkState>();
            return IsGranted(storageReadStatus) && IsGranted(storageWriteStatus) && IsGranted(cameraStatus) && IsGranted(photosStatus) && IsGranted(internetStatus);
#endif



        }

        private static async Task<PermissionStatus> CheckPermissions<TPermission>() where TPermission : Permissions.BasePermission, new()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<TPermission>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<TPermission>();
            }
            return status;
        }

        private static bool IsGranted(PermissionStatus status)
        {
            return status == PermissionStatus.Granted || status == PermissionStatus.Limited;
        }
    }
}

using System.Security.Permissions;
using CommunityToolKit.Maui;

namespace LocalEyesWebAPI.Helpers
{
    public class PermissionsChecker
    {
        private async Task<bool> CheckPermissions()
        {
            PermissionStatus bluetoothStatus = await CheckBluetoothPermissions();
            PermissionStatus cameraStatus = await CheckPermissions<Permissions.Camera>();
            PermissionStatus mediaStatus = await CheckPermissions<Permissions.Media>();
            PermissionStatus storageWriteStatus = await CheckPermissions<Permissions.StorageWrite>();
            //PermissionStatus photosStatus = await CheckPermissions<Permissions.Photos>();
            ...

        bool locationServices = IsLocationServiceEnabled();

            return IsGranted(cameraStatus) && IsGranted(mediaStatus) && IsGranted(storageWriteStatus) && IsGranted(bluetoothStatus);
        }

        private async Task<PermissionStatus> CheckBluetoothPermissions()
        {
            PermissionStatus bluetoothStatus = PermissionStatus.Granted;

            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                if (DeviceInfo.Version.Major >= 12)
                {
                    bluetoothStatus = await CheckPermissions<BluetoothPermissions>();
                }
                else
                {
                    bluetoothStatus = await CheckPermissions<Permissions.LocationWhenInUse>();
                }
            }

            return bluetoothStatus;
        }

        private async Task<PermissionStatus> CheckPermissions<TPermission>() where TPermission : Permissions.BasePermission, new()
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

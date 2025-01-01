namespace CutterManagement.UI.Desktop
{
    public enum IconKind
    {
        AppIcon, AppLogoIcon, WindowMinimizeIcon, WindowMaximizeIcon, WindowRestoreIcon, WindowCloseIcon, NotificationIcon, SpinnerIcon,
        SearchIcon, HomeIcon, InfosIcon, ArchivesIcon, UsersIcon, SettingsIcon, UpdatesIcon, AngleDownIcon, VerticalEllipsisIcon, WarningIcon,
        MaintenanceIcon, PasswordIcon, CheckMarkIcon, ShieldIcon
    }

    public static class IconHelpers
    {
        public static string GetIcon(this IconKind iconKind)
        {
            return iconKind switch
            {
                IconKind.AppIcon => "\uf0e7",
                IconKind.AppLogoIcon => "\uf0e7",
                IconKind.WindowMinimizeIcon => "\uf2d1",
                IconKind.WindowMaximizeIcon => "\uf0c8",
                IconKind.WindowRestoreIcon => "\uf24d",
                IconKind.WindowCloseIcon => "\ue59b",
                IconKind.NotificationIcon => "\uf0f3",
                IconKind.SpinnerIcon => "\ue1d4",
                IconKind.SearchIcon => "\uf002",
                IconKind.HomeIcon => "\ue487",
                IconKind.InfosIcon => "\uf05a",
                IconKind.ArchivesIcon => "\uf187",
                IconKind.UsersIcon => "\uf007",
                IconKind.SettingsIcon => "\uf013",
                IconKind.UpdatesIcon => "\uf09e",
                IconKind.AngleDownIcon => "\uf107",
                IconKind.VerticalEllipsisIcon => "\uf142",
                IconKind.WarningIcon => "\uf071",
                IconKind.MaintenanceIcon => "\uf7d9",
                IconKind.PasswordIcon => "\uf577",
                IconKind.CheckMarkIcon => "\uf00c",
                IconKind.ShieldIcon => "\uf132",
                _ => throw new NotImplementedException("Requested icon type is unknown")
            };
        }
    }
}

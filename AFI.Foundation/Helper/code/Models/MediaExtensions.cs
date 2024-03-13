using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;

namespace AFI.Foundation.Helper.Models
{
    public static class MediaExtensions
    {
        [System.Obsolete]
        public static string ImageUrl(this ImageField imageField, int? width = null, int? height = null)
        {
            if (imageField?.MediaItem == null)
                return string.Empty;
            MediaUrlOptions empty = MediaUrlOptions.Empty;
            int result;
            if (width.HasValue)
                empty.Width = width.Value;
            else if (int.TryParse(imageField.Width, out result))
                empty.Width = result;
            if (height.HasValue)
                empty.Height = height.Value;
            else if (int.TryParse(imageField.Height, out result))
                empty.Height = result;
            return imageField.ImageUrl(empty);
        }

        [System.Obsolete]
        public static string ImageUrl(this ImageField imageField, MediaUrlOptions options) => imageField?.MediaItem == null ? string.Empty : (options == null ? imageField.ImageUrl() : HashingUtils.ProtectAssetUrl(MediaManager.GetMediaUrl((MediaItem)imageField.MediaItem, options)));

        public static bool IsChecked(this Field checkboxField) => checkboxField != null && MainUtil.GetBool(checkboxField.Value, false);

    }
}
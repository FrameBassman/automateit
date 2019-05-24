using AutomateIt.Extensions.Extensions;

namespace AutomateIt.Framework.Browser
{
    public enum ECssProperty {
        [StringValue("left")]
        left,

        [StringValue("top")]
        top,

        [StringValue("width")]
        width,

        [StringValue("height")]
        height,

        [StringValue("background-image")]
        background_image,

        [StringValue("z-image")]
        zIndex,

        [StringValue("text-decoration")]
        text_decoration
    }
}
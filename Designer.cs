
namespace MachinVisionProgram
{
interface IBtnClick {
    void OnClick();
}

interface IShowImage {
    void ShowImage(string url);
}

interface IShowInformation {
    void ShowInformation(string information);
}

class Button {
    public string Text { get; set; } = "Button";
}

class ImageBox {
    public string TitleText { get; set; } = "ImageBox";
}

class TextBox {
    public string Text { get; set;} = "TextBox";
}
}
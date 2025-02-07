using ImageMagick.Drawing;

namespace CaptionMaker.Test;

// TODO: Finish adding the test cases
[TestClass]
public sealed class CaptionMakerTest
{
    [TestMethod]
    public void CaptionTextShorterThanTheImageWidthReturnsAsOneLine()
    {
        var captionMaker = new CaptionMaker(1080);

        Queue<IDrawables<byte>> captions = captionMaker.GenerateCaptions("Short caption");

        Assert.AreEqual(captions.Count, 1);
    }
}

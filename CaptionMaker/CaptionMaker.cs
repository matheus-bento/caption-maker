using ImageMagick;
using ImageMagick.Drawing;

namespace CaptionMaker
{
    public class CaptionMaker
    {
        /// <summary>
        ///     The width of the image where the caption will be drawn.
        ///     Used determine the need for line breaks
        /// </summary>
        private int _imageWidth = 0;

        public CaptionMaker(int imageWidth)
        {
            this._imageWidth = imageWidth;
        }

        /// <summary>
        ///     Calculates the width of a string according to the specified Magick.NET configurations
        /// </summary>
        /// <param name="captionDrawable">Object with the style used by Magick.NET to add the text to an image</param>
        /// <param name="text">The string that will be measured</param>
        private int GetTextWidth(IDrawables<byte> captionDrawable, string text)
        {
            return (int)Math.Floor(captionDrawable.FontTypeMetrics(text)?.TextWidth ?? 0);
        }

        /// <summary>
        ///     Generates objects that represents the caption lines that
        ///     can be drawn into a <see cref="ImageMagick.MagickImage" />
        /// </summary>
        /// <param name="text">The caption text</param>
        public Queue<IDrawables<byte>> GenerateCaptions(string text)
        {
            var captions = new Queue<IDrawables<byte>>();

            int halfWidth = this._imageWidth / 2;
            int lineHeight = 80;

            var captionImageStyle =
                new Drawables()
                    .FontPointSize(72)
                    .Font("./static/Ubuntu.ttf")
                    .StrokeColor(MagickColors.Black)
                    .FillColor(MagickColors.White)
                    .TextAlignment(TextAlignment.Center);

            int captionWidth = this.GetTextWidth(captionImageStyle, text);

            var captionChunks = new Queue<string>();

            if (captionWidth <= this._imageWidth)
            {
                // If the text already fits in the image, we simply push into the queue and return
                // it without further manipulation
                captionChunks.Enqueue(text);
            }
            else
            {
                string remainingText = text;

                // If the text does not fit in the image, we split it into chunks with the maximum
                // width equal to the image's width
                do
                {
                    // TODO: Guard against the edge case of a string without any whitespace

                    // Tries to cut off a chunk of text from the caption that fits within the image
                    int chunkEnd = 0;

                    while (
                        chunkEnd < remainingText.Length &&
                        this.GetTextWidth(captionImageStyle, remainingText.Substring(0, chunkEnd + 1)) < this._imageWidth
                    )
                    {
                        chunkEnd++;
                    }

                    // If the final character of the chunk is not a whitespace, it keeps going back until a
                    // whitespace is found and cuts the chunk off at that position in order to avoid cutting
                    // a word in half when breaking the captions up into several chunks

                    while (chunkEnd < remainingText.Length && !Char.IsWhiteSpace(remainingText[chunkEnd]))
                    {
                        chunkEnd--;
                    }

                    string chunk = remainingText.Substring(0, chunkEnd).Trim();

                    captionChunks.Enqueue(chunk);

                    // Removes the chunk from the remaining of the caption text and recalculates its width
                    remainingText = remainingText.Substring(chunkEnd);
                }
                while (!String.IsNullOrEmpty(remainingText));


            }

            int lineCount = 1;

            while (captionChunks.Count > 0)
            {
                string currLine = captionChunks.Dequeue();

                IDrawables<byte> partialCaption =
                    new Drawables()
                        .FontPointSize(72)
                        .Font("./static/Ubuntu.ttf")
                        .StrokeColor(MagickColors.Black)
                        .FillColor(MagickColors.White)
                        .TextAlignment(TextAlignment.Center)
                        .Text(halfWidth, lineHeight * lineCount, currLine);

                captions.Enqueue(partialCaption);

                lineCount++;
            }

            return captions;
        }
    }
}

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace SignatureProject
{
    //Summary: Convert signature from InkCanvas to Bitmap, then Bitmap to jpg

    public class MainViewModel
    {
        private StrokeCollection _signature { get; set; }
        public virtual StrokeCollection Signature
        {
            get { return _signature ?? (_signature = new StrokeCollection()); }
        set { _signature = value; }
        }

        public void Clear()
        {
            //erases strokes from Inkcanvas
            Signature.Clear();
        }

        public void ConvertToBitmap()
        {
            //create source for rendering-> strokecollection
            DrawingVisual drawingVisual = new DrawingVisual();
            
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                Signature.Draw(drawingContext);
            }

            //Convert visual to bitmap
            RenderTargetBitmap rtb = new RenderTargetBitmap(525, 183, 96d, 96d, PixelFormats.Default);
            rtb.Render(drawingVisual);
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            //Save Signature
            OpenFileDialog(encoder);
        }

        public void OpenFileDialog(BitmapEncoder e)
        {
            //open file dialog
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "PNG Files | *.png";
            dlg.DefaultExt = "png";
            bool? result = dlg.ShowDialog();

            //get file name
            string filename = dlg.FileName;

            if (result == true)
            {
                using (var ms = new MemoryStream())
                {
                    e.Save(ms);
                    FixBlackBackground(ms, filename);
                }
            }
        }

        public void FixBlackBackground(MemoryStream ms, string filename)
        {   
            using (Image img = Image.FromStream(ms))
            {
                using (var b = new Bitmap(img.Width, img.Height))
                {
                    b.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                    //change background to white
                    using (var g = Graphics.FromImage(b))
                    {
                        g.Clear(Color.White);
                        g.DrawImageUnscaled(img, 0, 0);

                        b.Save(filename, ImageFormat.Png);
                    }
                }
            }
        }
    }
}

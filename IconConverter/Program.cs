using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

var pngPath = args[0];
var icoPath = args[1];

using var img = Image.FromFile(pngPath);
using var bmp = new Bitmap(img, new Size(256, 256));
using var stream = new FileStream(icoPath, FileMode.Create);

var icon = Icon.FromHandle(bmp.GetHicon());
icon.Save(stream);

Console.WriteLine($"Converted {pngPath} to {icoPath}");

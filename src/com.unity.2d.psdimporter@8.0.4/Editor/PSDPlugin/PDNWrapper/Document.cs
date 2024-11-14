using System.Collections;
using System.Collections.Generic;
using PhotoshopFile;

namespace PDNWrapper
{
    internal class Document
    {
        public           int        width, height;
        // test2
        public  GuidInfo[] GuidesInfoGuides;

        public Document(int w, int h, GuidInfo[] guidesInfoGuides)
        {
            width = w;
            height = h;
            GuidesInfoGuides = guidesInfoGuides;
            Layers = new List<BitmapLayer>();
        }

        public void Dispose()
        {
            foreach (var layer in Layers)
                layer.Dispose();
        }

        public List<BitmapLayer> Layers { get; set; }

        public MeasurementUnit DpuUnit { get; set; }

        public double DpuX { get; set; }
        public double DpuY { get; set; }
    }
}

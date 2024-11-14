namespace PhotoshopFile
{
    // test2
    internal class GridGuidesInfo : ImageResource
    {
        public          uint       Version    { get; set; }
        public          uint       Rows       { get; set; }
        public          uint       Cols       { get; set; }
        public          uint       GuideCount { get; set; }
        public          GuidInfo[] Guides     { get; set; }
        public override ResourceID ID         => ResourceID.GridGuidesInfo;

        public GridGuidesInfo(PsdBinaryReader reader, string name, int resourceDataLength)
            : base(name)
        {
            var endPosition = reader.BaseStream.Position + resourceDataLength;

            // 读二进制数据 https://www.adobe.com/devnet-apps/photoshop/fileformatashtml/#50577409_62190
            Version = reader.ReadUInt32();
            Rows = reader.ReadUInt32();
            Cols = reader.ReadUInt32();
            GuideCount = reader.ReadUInt32();
            if (GuideCount <= 0)
                return;
            
            Guides = new GuidInfo[GuideCount];
            var index = 0;
            while (reader.BaseStream.Position < endPosition)
            {
                Guides[index] = new GuidInfo{
                    Pos = (reader.ReadUInt32() >> 5),   // 读取之后要转成小数，右移5位
                    Direction = reader.ReadByte()
                };
                index++;
            }
        }
        
    }

    internal class GuidInfo
    {
        public float Pos       {get; set; }
        public byte Direction { get; set; }
    }
}
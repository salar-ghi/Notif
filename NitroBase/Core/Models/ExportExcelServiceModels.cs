using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class DisplayFormatAttribute : Attribute
    {
        public DisplayFormatAttribute(string format)
        {
            Format = format;
        }

        public string Format { get; set; }
    }

    public class Header
    {
        public List<HeaderRow> Rows { get; set; } = new();
    }

    public class HeaderRow
    {
        public List<HeaderColumn> Columns { get; set; } = new();
        public byte Height { get; set; } = 0;

    }

    public class Footer
    {
        public List<FooterRow> Rows { get; set; } = new();
    }

    public class FooterRow
    {
        public List<FooterColumn> Columns { get; set; } = new();

    }

    public class FooterColumn
    {
        /// <summary>
        /// ColumnIndex starts from 1
        /// </summary>
        public int ColumnIndex { get; set; }
        public Color BackgroundColor { get; set; }
        public object Value { get; set; }

    }

    public class Color
    {
        public Color() { }
        public Color(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
    }

    public class HeaderColumn
    {
        public HeaderColumn() { }
        public HeaderColumn(string title = "", int colSpan = 1, int rowSpan = 1, int width = 0, Color backgroundColor = null)
        {
            Title = title;
            ColSpan = colSpan;
            RowSpan = rowSpan;
            Width = width;
            BackgroundColor = backgroundColor;
        }
        public string Title { get; set; }
        public int ColSpan { get; set; } = 1;
        public int RowSpan { get; set; } = 1;
        public int Width { get; set; } = 0;
        public Color BackgroundColor { get; set; }

    }

}

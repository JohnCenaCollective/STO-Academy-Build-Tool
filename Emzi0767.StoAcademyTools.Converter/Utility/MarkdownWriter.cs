using System;
using System.IO;
using md = Html2Markdown;

namespace Emzi0767.StoAcademyTools.Converter.Utility
{
    public class MarkdownWriter : IDisposable
    {
        private TextWriter TextWriter { get; set; }
        private bool InTable { get; set; }
        private bool InRow { get; set; }
        private int ColCount { get; set; }
        private int ColCurrent { get; set; }
        private bool HeaderWritten { get; set; }
        private bool AlignWritten { get; set; }

        public MarkdownWriter(TextWriter tw)
        {
            this.TextWriter = tw;
            this.InTable = false;
            this.InRow = false;
            this.ColCount = 0;
            this.ColCurrent = 0;
            this.HeaderWritten = false;
            this.AlignWritten = false;
        }

        public void WriteParagraph(string paragraph)
        {
            if (this.InTable)
                throw new InvalidOperationException("Attempted to write non-table content in table mode.");

            this.TextWriter.WriteLine(paragraph);
            this.TextWriter.WriteLine();
        }

        public void WriteRule()
        {
            if (this.InTable)
                throw new InvalidOperationException("Attempted to write non-table content in table mode.");

            this.TextWriter.WriteLine("---");
            this.TextWriter.WriteLine();
        }

        public void WriteHeader(string text, int level)
        {
            if (this.InTable)
                throw new InvalidOperationException("Attempted to write non-table content in table mode.");

            if (level < 1 || level > 6)
                throw new ArgumentException("Invalid header level");

            for (int i = 0; i < level; i++)
                this.TextWriter.Write("#");
            this.TextWriter.WriteLine(text);
            this.TextWriter.WriteLine();
        }

        public void WriteHeaderLink(string text, Uri url, int level)
        {
            if (this.InTable)
                throw new InvalidOperationException("Attempted to write non-table content in table mode.");

            if (level < 1 || level > 6)
                throw new ArgumentException("Invalid header level");

            for (int i = 0; i < level; i++)
                this.TextWriter.Write("#");
            this.WriteLink(text, url);
            this.TextWriter.WriteLine();
            this.TextWriter.WriteLine();
        }

        public void WriteLink(string text, Uri url)
        {
            this.TextWriter.Write("[{0}]({1})", text, url);
        }

        public void WriteFormat(string text, bool bold, bool italic, bool strike)
        {
            if (bold)
                this.TextWriter.Write("**");
            if (italic)
                this.TextWriter.Write("*");
            if (strike)
                this.TextWriter.Write("~~");

            this.TextWriter.Write(text);

            if (strike)
                this.TextWriter.Write("~~");
            if (italic)
                this.TextWriter.Write("*");
            if (bold)
                this.TextWriter.Write("**");
        }

        public void WriteHtml(string html)
        {
            if (this.InTable)
                throw new InvalidOperationException("Attempted to write non-table content in table mode.");
            
            var mdcv = new md.Converter();
            var md = mdcv.Convert(html);

            this.TextWriter.WriteLine(md);
            this.TextWriter.WriteLine();
        }

        public void WriteTable(string[] headers, string[] alignments, params string[][] rows)
        {
            if (this.InTable)
                throw new InvalidOperationException("Attempted to write non-table content in table mode.");

            if (headers.Length != alignments.Length)
                throw new ArgumentException("Table header count must be the same as alignment count");

            var maxlen = headers.Length;
            for (int i = 0; i < maxlen; i++)
            {
                this.WriteFormat(headers[i], true, false, false);
                if (i + 1 != maxlen)
                    this.TextWriter.Write(" | ");
            }
            this.TextWriter.WriteLine();

            for (int i = 0; i < maxlen; i++)
            {
                switch (alignments[i].ToLower())
                {
                    case "left":
                    default:
                        this.TextWriter.Write(":--");
                        break;

                    case "right":
                        this.TextWriter.Write("--:");
                        break;

                    case "center":
                        this.TextWriter.Write(":--:");
                        break;
                }
                if (i + 1 != maxlen)
                    this.TextWriter.Write("|");
            }
            this.TextWriter.WriteLine();

            foreach (var row in rows)
            {
                if (row.Length != maxlen)
                    throw new ArgumentException("Rows must all be same length");

                for (int i = 0; i < maxlen; i++)
                {
                    this.TextWriter.Write(row[i]);
                    if (i + 1 != maxlen)
                        this.TextWriter.Write(" | ");
                }
                this.TextWriter.WriteLine();
            }

            this.TextWriter.WriteLine();
        }

        public void StartTable()
        {
            if (this.InTable)
                throw new InvalidOperationException("Table was already started");

            this.InTable = true;
        }

        public void EndTable()
        {
            if (!this.InTable)
                throw new InvalidOperationException("No table was started");

            this.InTable = false;
            this.ColCount = 0;
            this.HeaderWritten = false;
            this.AlignWritten = false;
            this.TextWriter.WriteLine();
        }

        public void WriteTableHeaders(string[] headers)
        {
            if (!this.InTable)
                throw new InvalidOperationException("You need to be in table mode to write table headers");

            if (this.HeaderWritten)
                throw new InvalidOperationException("Table headers were already written");

            this.ColCount = headers.Length;

            for (int i = 0; i < this.ColCount; i++)
            {
                this.WriteFormat(headers[i], true, false, false);
                if (i + 1 < this.ColCount)
                    this.TextWriter.Write(" | ");
            }
            this.TextWriter.WriteLine();

            this.HeaderWritten = true;
        }

        public void WriteTableAlignments(string[] alignments)
        {
            if (!this.InTable)
                throw new InvalidOperationException("You need to be in table mode to write table headers");

            if (!this.HeaderWritten)
                throw new InvalidOperationException("You need to write table headers first");

            if (this.AlignWritten)
                throw new InvalidOperationException("Table alignments were already written");

            if (alignments.Length != this.ColCount)
                throw new ArgumentException("Invalid alignment count");

            for (int i = 0; i < this.ColCount; i++)
            {
                switch (alignments[i].ToLower())
                {
                    case "left":
                    default:
                        this.TextWriter.Write(":--");
                        break;

                    case "right":
                        this.TextWriter.Write("--:");
                        break;

                    case "center":
                        this.TextWriter.Write(":--:");
                        break;
                }
                if (i + 1 < this.ColCount)
                    this.TextWriter.Write("|");
            }
            this.TextWriter.WriteLine();

            this.AlignWritten = true;
        }

        public void StartTableRow()
        {
            if (!this.InTable)
                throw new InvalidOperationException("You need to be in table mode to write rows");

            if (this.InRow)
                throw new InvalidOperationException("Table row was already started");

            if (!this.HeaderWritten || !this.AlignWritten)
                throw new InvalidOperationException("You need to write table header and alignments before you write table's contents");

            this.InRow = true;
            this.ColCurrent = 0;
        }

        public void EndTableRow()
        {
            if (!this.InTable)
                throw new InvalidOperationException("You need to be in table mode to write rows");

            if (!this.InRow)
                throw new InvalidOperationException("Table row was not started");

            this.InRow = false;
            this.ColCurrent = 0;
            this.TextWriter.WriteLine();
        }

        public void WriteTableCell(string text)
        {
            if (!this.InTable)
                throw new InvalidOperationException("You need to be in table mode to write rows");

            if (!this.InRow)
                throw new InvalidOperationException("You need to be in table row mode to write rows");

            if (this.ColCurrent >= this.ColCount)
                throw new InvalidOperationException("Attempted to write too many cells");

            this.TextWriter.Write(text);
            if (this.ColCurrent + 1 != this.ColCount)
                this.TextWriter.Write(" | ");

            this.ColCurrent++;
        }

        public void Flush()
        {
            this.TextWriter.Flush();
        }

        public void Dispose()
        {
            this.TextWriter.Dispose();   
        }
    }
}

using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace QnSNote
{
    public partial class Form1 : Form
    {
        private enum NoteAreas
        {
            Item,
            Context
        };

        private NoteAreas _highlightedArea = NoteAreas.Item;
        private int _highlightedNum;
        private TextBox _movingItem;
        private string _lastSavedSerialization = "";

        public Form1()
        {
            // This call is required by the designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
        }

        private void BroadcastKey(string key)
        {
            SendKeys.Send(key);
        }

        private void Highlight(NoteAreas area, int order)
        {
            Unhighlight();
            _highlightedArea = area;
            _highlightedNum = order;
            if (area == NoteAreas.Item)
            {
                var item = (TextBox) _lay.GetControlFromPosition(0, order);
                item.BackColor = Color.Aquamarine;
            }
            else
            {
                // something...
            }
        }

        private TextBox CreateNewTextBox(string text = "")
        {
            var textBox = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                Height = 25,
                Font = new Font("맑은 고딕", 10.0f),
                ImeMode = ImeMode.Hangul,
                Text = text,
            };

            textBox.TextChanged += AdjustTextBoxSize;
            textBox.KeyDown += Item_KeyDown;
            textBox.KeyUp += Item_KeyUp;
            textBox.GotFocus += Item_GotFocus;
            textBox.LostFocus += Item_LostFocus;
            return textBox;
        }

        private TextBox CopyTextBox(TextBox textBox)
        {
            var copiedItem = CreateNewTextBox();
            copiedItem.Tag = _lay.RowCount;
            copiedItem.ImeMode = ImeMode.Hangul;
            copiedItem.Text = textBox.Text;
            copiedItem.BackColor = Color.PaleVioletRed;
            return copiedItem;
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            var textBox = CreateNewTextBox();
            _lay.Controls.Add(textBox, 0, _lay.RowCount);
            _lay.RowCount += 1;
            textBox.Select();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            ActiveControl = null;
            Close();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = false;
            switch (e.KeyCode)
            {
                case Keys.Up:
                {
                    var highlightedItem = (TextBox) _lay.GetControlFromPosition(0, _highlightedNum);
                    if (!(e.Alt && e.Shift))
                    {
                        if (_highlightedNum > 0)
                            Highlight(_highlightedArea, _highlightedNum - 1);
                    }
                    else
                    {
                        if (!e.Control)
                        {
                            if (_highlightedNum > 0)
                            {
                                Swap(highlightedItem, _lay.GetControlFromPosition(0, _highlightedNum - 1));
                                Highlight(_highlightedArea, _highlightedNum - 1);
                                _movingItem = highlightedItem;
                            }
                        }
                        else
                        {
                            if (_movingItem != null)
                            {
                                // ctrl+alt+shift-up moving
                                if (_highlightedNum > 0)
                                {
                                    Swap(highlightedItem, _lay.GetControlFromPosition(0, _highlightedNum - 1));
                                    Highlight(_highlightedArea, _highlightedNum - 1);
                                }
                            }
                            else
                            {
                                // ctrl+alt+shift-up new
                                if (highlightedItem.Text != "")
                                {
                                    var copiedItem = CopyTextBox(highlightedItem);
                                    Insert(copiedItem, _lay.GetRow(highlightedItem));
                                    _movingItem = copiedItem;
                                    Highlight(_highlightedArea, _highlightedNum - 1);
                                }
                            }
                        }
                    }

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
                }

                case Keys.Down:
                {
                    var highlightedItem = (TextBox) _lay.GetControlFromPosition(0, _highlightedNum);
                    if (!(e.Alt && e.Shift))
                    {
                        if (_highlightedNum < _lay.RowCount - 1)
                            Highlight(_highlightedArea, _highlightedNum + 1);
                    }
                    else
                    {
                        if (!e.Control)
                        {
                            if (_highlightedNum < _lay.RowCount - 1)
                            {
                                Swap(highlightedItem, _lay.GetControlFromPosition(0, _highlightedNum + 1));
                                Highlight(_highlightedArea, _highlightedNum + 1);
                                _movingItem = highlightedItem;
                            }
                        }
                        else
                        {
                            if (_movingItem != null)
                            {
                                // ctrl+alt+shift-down moving
                                if (_highlightedNum < _lay.RowCount - 1)
                                {
                                    Swap(highlightedItem, _lay.GetControlFromPosition(0, _highlightedNum + 1));
                                    Highlight(_highlightedArea, _highlightedNum + 1);
                                }
                            }
                            else
                            {
                                // ctrl+alt+shift-down new
                                if (highlightedItem.Text != "")
                                {
                                    var copiedItem = CopyTextBox(highlightedItem);
                                    Insert(copiedItem, _lay.GetRow(highlightedItem) + 1);
                                    _movingItem = copiedItem;
                                    Highlight(_highlightedArea, _highlightedNum + 1);
                                }
                            }
                        }
                    }

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
                }
                case Keys.F2:
                case Keys.Right:
                {
                    var textBox = (TextBox) _lay.GetControlFromPosition(0, _highlightedNum);
                    textBox.SelectionStart = textBox.TextLength;
                    _lay.GetControlFromPosition(0, _highlightedNum).Select();
                    break;
                }
                case Keys.Left:
                {
                    var textBox = (TextBox) _lay.GetControlFromPosition(0, _highlightedNum);
                    textBox.SelectionStart = textBox.TextLength - 1;
                    _lay.GetControlFromPosition(0, _highlightedNum).Select();
                    break;
                }
                case Keys.Return:
                {
                    var textBox = (TextBox) _lay.GetControlFromPosition(0, _highlightedNum);
                    if (textBox.TextLength > 0)
                    {
                        var newItem = CreateNewTextBox();
                        if (!e.Shift)
                            Insert(newItem, _highlightedNum + 1);
                        else
                            Insert(newItem, _highlightedNum);

                        newItem.Select();
                    }

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
                }
                case Keys.Delete:
                    Remove(_lay, _lay.GetControlFromPosition(0, _highlightedNum));
                    if (_highlightedNum == _lay.RowCount)
                    {
                        Highlight(_highlightedArea, _highlightedNum - 1);
                    }
                    else if (_lay.RowCount == 1)
                    {
                        _lay.Controls[0].Text = "";
                        Highlight(_highlightedArea, _highlightedNum);
                    }

                    e.Handled = true;
                    break;
                case Keys.Escape:
                    Close();
                    break;
                case Keys.C:
                    if (e.Control && !e.Alt && !e.Shift)
                    {
                        var highlightedItem = (TextBox) _lay.GetControlFromPosition(0, _highlightedNum);
                        Clipboard.SetText(highlightedItem.Text);
                        highlightedItem.Select();
                        e.SuppressKeyPress = true;
                        e.Handled = true;
                    }

                    break;
                case Keys.X:
                    if (e.Control && !e.Alt && !e.Shift)
                    {
                        var highlightedItem = (TextBox) _lay.GetControlFromPosition(0, _highlightedNum);
                        Clipboard.SetText(highlightedItem.Text);
                        highlightedItem.Select();
                        highlightedItem.Text = "";
                        e.SuppressKeyPress = true;
                        e.Handled = true;
                    }

                    break;
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Return)
                return;

            var highlightedItem = (TextBox) _lay.GetControlFromPosition(0, _highlightedNum);
            highlightedItem.Focus();
            highlightedItem.SelectionStart = highlightedItem.TextLength;
            BroadcastKey(e.KeyChar.ToString());
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (_movingItem == null || e.Alt && e.Shift)
                return;

            Highlight(_highlightedArea, _lay.GetRow(_movingItem));
            _movingItem = null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var resumeSaved = false;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(Screen.PrimaryScreen.Bounds.Width - Size.Width,
                (int) (Screen.PrimaryScreen.Bounds.Height * 0.1));
            if (File.Exists("QnS.ini"))
            {
                if (new FileInfo("QnS.ini").Length > 0)
                    resumeSaved = true;
            }

            if (!resumeSaved)
            {
                var textBox = CreateNewTextBox();
                _lay.Controls.Add(textBox, 0, 0);
                textBox.Focus();
            }
            else
            {
                var savedLines = File.ReadAllText("QnS.ini");
                _lay.RowCount = 0;
                foreach (var line in savedLines.Split(new[] {"\r\n", "\r", "\n"},
                    StringSplitOptions.RemoveEmptyEntries))
                {
                    var textBox = CreateNewTextBox(line);
                    _lay.Controls.Add(textBox, 0, _lay.RowCount);
                    _lay.RowCount += 1;
                    AdjustTextBoxSize(textBox, null);
                }

                _lay.GetControlFromPosition(0, _lay.RowCount - 1).Focus();
                _lastSavedSerialization = savedLines;
            }
        }

        private void Insert(Control controlToInsert, int at = 1)
        {
            _lay.RowCount += 1;

            var enumerator = _lay.Controls.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var current = (Control) enumerator.Current;
                if (_lay.GetRow(current) >= at)
                    _lay.SetRow(current, _lay.GetRow(current) + 1);
            }

            if (_highlightedNum >= at)
                _highlightedNum += 1;

            _lay.Controls.Add(controlToInsert, 0, at);
        }

        private void Item_GotFocus(object sender, EventArgs e)
        {
            Unhighlight();
            if (sender == _movingItem)
                return;

            var textBox = (TextBox) sender;
            textBox.BackColor = Color.AliceBlue;
        }

        private void Item_KeyDown(object sender, KeyEventArgs e)
        {
            var textBox = (TextBox) sender;
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    ActiveControl = null;
                    if (textBox.TextLength != 0 || _lay.GetRow(textBox) <= 0)
                        Highlight(NoteAreas.Item, _lay.GetRow(textBox));
                    else
                        Highlight(NoteAreas.Item, _lay.GetRow(textBox) - 1);

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Return:
                    if (textBox.TextLength > 0)
                    {
                        var newItem = CreateNewTextBox();
                        if (!e.Shift)
                            Insert(newItem, _lay.GetRow(textBox) + 1);
                        else
                            Insert(newItem, _lay.GetRow(textBox));

                        newItem.Focus();
                    }

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Up:
                {
                    var itemNum = _lay.GetRow(textBox);
                    if (e.Alt && e.Shift)
                    {
                        if (!e.Control)
                        {
                            Swap(textBox, _lay.GetControlFromPosition(0, itemNum - 1));
                            _movingItem = textBox;
                        }
                        else
                        {
                            if (_movingItem != null)
                            {
                                // ctrl+alt+shift-up moving
                                if (itemNum > 0)
                                    Swap(textBox, _lay.GetControlFromPosition(0, itemNum - 1));
                            }
                            else
                            {
                                // ctrl+alt+shift-up new
                                if (textBox.Text != "")
                                {
                                    var copiedItem = CopyTextBox(textBox);
                                    Insert(copiedItem, _lay.GetRow(textBox));
                                    copiedItem.Focus();
                                    _movingItem = copiedItem;
                                }
                            }
                        }

                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    else if (textBox.GetLineFromCharIndex(textBox.SelectionStart) == 0 && itemNum > 0)
                    {
                        Highlight(_highlightedArea, itemNum - 1);
                        ActiveControl = null;
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }

                    break;
                }
                case Keys.Down:
                {
                    var itemNum = _lay.GetRow(textBox);
                    if (e.Alt && e.Shift)
                    {
                        if (!e.Control)
                        {
                            if (itemNum < _lay.RowCount - 1)
                            {
                                Swap(textBox, _lay.GetControlFromPosition(0, itemNum + 1));
                                _movingItem = textBox;
                            }
                        }
                        else
                        {
                            if (_movingItem != null)
                            {
                                // ctrl+alt+shift-down moving
                                if (itemNum < _lay.RowCount - 1)
                                    Swap(textBox, _lay.GetControlFromPosition(0, itemNum + 1));
                            }
                            else
                            {
                                // ctrl+alt+shift-down new
                                if (textBox.Text != "")
                                {
                                    var copiedItem = CopyTextBox(textBox);
                                    Insert(copiedItem, _lay.GetRow(textBox) + 1);
                                    copiedItem.Focus();
                                    _movingItem = copiedItem;
                                }
                            }
                        }

                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    else if (textBox.GetLineFromCharIndex(textBox.SelectionStart) >= textBox.GetLineFromCharIndex(textBox.Text.Length - 1)
                             && itemNum < _lay.RowCount - 1)
                    {
                        Highlight(_highlightedArea, itemNum + 1);
                        ActiveControl = null;
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }

                    break;
                }
            }
        }

        private void Item_KeyUp(object sender, KeyEventArgs e)
        {
            if (_movingItem == null)
                return;

            if (e.Alt && e.Shift)
                return;

            var textBox = _movingItem;
            textBox.BackColor = Color.AliceBlue;
            _movingItem = null;
            e.Handled = true;
        }

        private void Item_LostFocus(object sender, EventArgs e)
        {
            var textBox = (TextBox) sender;
            textBox.BackColor = Color.White;
            if (textBox.TextLength == 0)
                Remove(_lay, textBox);

            if (_lay.Controls.Count == _lay.RowCount)
                SaveAll();
        }

        private void AdjustTextBoxSize(object sender, EventArgs e)
        {
            var textBox = (TextBox) sender;
            var numberOfLine = textBox.GetLineFromCharIndex(textBox.TextLength) + 1;
            var outerMargin = textBox.Height - textBox.ClientSize.Height;
            textBox.Height = textBox.Font.Height * numberOfLine + 3 + outerMargin;
        }

        private void Remove(TableLayoutPanel layout, Control controlToInsert)
        {
            if (_lay.RowCount == 1)
                return;

            var rowToRemove = _lay.GetRow(controlToInsert);

            // 레이아웃에서 제거
            layout.Controls.Remove(controlToInsert);

            // 밑에 있는 칸들 위로 밀기
            var enumerator = layout.Controls.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var current = (TextBox) enumerator.Current;
                if (layout.GetRow(current) >= rowToRemove)
                    layout.SetRow(current, layout.GetRow(current) - 1);
            }

            layout.RowCount -= 1;
            if (_highlightedNum > rowToRemove || _highlightedNum >= layout.RowCount)
                Highlight(_highlightedArea, _highlightedNum - 1);
            else if (_highlightedNum == rowToRemove)
                Highlight(_highlightedArea, _highlightedNum);

            if (_lay.Controls.Count == _lay.RowCount)
                SaveAll();
        }

        private void SaveAll()
        {
            var stringBuilder = new StringBuilder();
            for (var itemNum = 0; itemNum < _lay.RowCount; ++itemNum)
            {
                var controlFromPosition = _lay.GetControlFromPosition(0, itemNum) as TextBox;
                if (controlFromPosition.TextLength > 0)
                    stringBuilder.AppendLine(controlFromPosition.Text);
            }

            var serialized = stringBuilder.ToString();
            if (serialized == _lastSavedSerialization)
                return;

            using (var streamWriter = new StreamWriter("QnS.ini"))
            {
                streamWriter.Write(serialized);
            }

            _lastSavedSerialization = serialized;
        }

        private void Swap(Control a, Control b)
        {
            var aRow = _lay.GetRow(a);
            _lay.SetRow(a, _lay.GetRow(b));
            _lay.SetRow(b, aRow);
            SaveAll();
        }

        private void Unhighlight()
        {
            if (_highlightedNum >= _lay.RowCount)
                return;

            if (_highlightedArea != NoteAreas.Item)
                return;

            var highlightedItem = (TextBox) _lay.GetControlFromPosition(0, _highlightedNum);
            highlightedItem.BackColor = Color.White;
        }
    }
}
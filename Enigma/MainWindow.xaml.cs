using Enigma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Enigma
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Rotor rightRotor, middleRotor, leftRotor, reflector;
        private List<Pair> pairs;
        private List<Rotor> rotors;

        public MainWindow()
        {
            InitializeComponent();

            rotors = new List<Rotor>();
            InitRotors();

            pairs = new List<Pair>();

            ShowHideAvilableRotors();

            UpdateOffsetLabel();
        }

        public void InitRotors()
        {
            //--------------------ABCDEFGHIJKLMNOPQRSTUVWXYZ---------//
            rotors.Add(new Rotor("EKMFLGDQVZNTOWYHXUSPAIBRCJ", 'A', 'Q', 1, false, 'A'));
            rotors.Add(new Rotor("AJDKSIRUXBLHWTMCQGZNPYFVOE", 'A', 'E', 2, false, 'A'));
            rotors.Add(new Rotor("BDFHJLCPRTXVZNYEIWGAKMUSQO", 'A', 'V', 3, false, 'A'));
            rotors.Add(new Rotor("ESOVPZJAYQUIRHXLNFTGKDCMWB", 'A', 'J', 4, false, 'A'));
            rotors.Add(new Rotor("VZBRGITYUPSDNHLXAWMJQOFECK", 'A', 'Z', 5, false, 'A'));

            rightRotor = rotors[2];
            middleRotor = rotors[1];
            leftRotor = rotors[0];
            reflector = new Rotor("YRUHQSLDPXNGOKMIEBFZCWVJAT", 'A', '\0', 0, true, 'A');

            SetRotorsPosition();

            for (int i = 1; i <= 5; i++)
            {
                cmbLeftRotor.Items.Add(new ComboBoxItem() { Content = i.ToString() });
                cmbMiddleRotor.Items.Add(new ComboBoxItem() { Content = i.ToString() });
                cmbRightRotor.Items.Add(new ComboBoxItem() { Content = i.ToString() });
            }

            cmbRightRotor.SelectedIndex = rightRotor.RotorNumber - 1;
            cmbMiddleRotor.SelectedIndex = middleRotor.RotorNumber - 1;
            cmbLeftRotor.SelectedIndex = leftRotor.RotorNumber - 1;
        }

        public void SetRotorsPosition()
        {
            rightRotor.PreviousRotor = null;
            rightRotor.NextRotor = middleRotor;
            middleRotor.PreviousRotor = rightRotor;
            middleRotor.NextRotor = leftRotor;
            leftRotor.PreviousRotor = middleRotor;
            leftRotor.NextRotor = reflector;
            reflector.PreviousRotor = leftRotor;
            reflector.NextRotor = null;
        }

        public void UpdateOffsetLabel()
        {
            lblRightRotor.Content = rightRotor.Offset;
            lblRightRotorRing.Content = rightRotor.InnerRingSetting;
            lblMiddleRotor.Content = middleRotor.Offset;
            lblMiddleRotorRing.Content = middleRotor.InnerRingSetting;
            lblLeftRotor.Content = leftRotor.Offset;
            lblLeftRotorRing.Content = leftRotor.InnerRingSetting;

        }

        public void ShowHideAvilableRotors()
        {
            string selectedRight = "";
            string selectedMiddle = "";
            string selectedLeft = "";

            if (cmbRightRotor != null && cmbRightRotor.Items.Count > 0 && cmbRightRotor.SelectedItem != null)
                selectedRight = ((ComboBoxItem)cmbRightRotor.SelectedItem).Content.ToString();

            if (cmbMiddleRotor != null && cmbMiddleRotor.Items.Count > 0 && cmbMiddleRotor.SelectedItem != null)
                selectedMiddle = ((ComboBoxItem)cmbMiddleRotor.SelectedItem).Content.ToString();

            if (cmbLeftRotor != null && cmbLeftRotor.Items.Count > 0 && cmbLeftRotor.SelectedItem != null)
                selectedLeft = ((ComboBoxItem)cmbLeftRotor.SelectedItem).Content.ToString();


            if (cmbRightRotor != null && cmbRightRotor.Items.Count > 0 && cmbRightRotor.SelectedItem != null)
            {
                foreach (ComboBoxItem item in cmbRightRotor.Items)
                {
                    if (item.Content.ToString() == selectedLeft || item.Content.ToString() == selectedMiddle)
                        item.IsEnabled = false;
                    else
                        item.IsEnabled = true;
                }
            }

            if (cmbMiddleRotor != null && cmbMiddleRotor.Items.Count > 0 && cmbMiddleRotor.SelectedItem != null)
            {
                foreach (ComboBoxItem item in cmbMiddleRotor.Items)
                {
                    if (item.Content.ToString() == selectedLeft || item.Content.ToString() == selectedRight)
                        item.IsEnabled = false;
                    else
                        item.IsEnabled = true;
                }
            }

            if (cmbLeftRotor != null && cmbLeftRotor.Items.Count > 0 && cmbLeftRotor.SelectedItem != null)
            {
                foreach (ComboBoxItem item in cmbLeftRotor.Items)
                {
                    if (item.Content.ToString() == selectedMiddle || item.Content.ToString() == selectedRight)
                        item.IsEnabled = false;
                    else
                        item.IsEnabled = true;
                }
            }
        }

        private void btnRightUp_Click(object sender, RoutedEventArgs e)
        {
            rightRotor.MoveUp(false);
            UpdateOffsetLabel();
            ResetText();
        }

        private void btnMiddleUp_Click(object sender, RoutedEventArgs e)
        {
            middleRotor.MoveUp(false);
            UpdateOffsetLabel();
            ResetText();
        }

        private void btnLeftUp_Click(object sender, RoutedEventArgs e)
        {
            leftRotor.MoveUp(false);
            UpdateOffsetLabel();
            ResetText();
        }

        private void btnRightDown_Click(object sender, RoutedEventArgs e)
        {
            rightRotor.MoveDown();
            UpdateOffsetLabel();
            ResetText();
        }

        private void btnMiddleDown_Click(object sender, RoutedEventArgs e)
        {
            middleRotor.MoveDown();
            UpdateOffsetLabel();
            ResetText();
        }

        private void btnLeftDown_Click(object sender, RoutedEventArgs e)
        {
            leftRotor.MoveDown();
            UpdateOffsetLabel();
            ResetText();
        }

        private void btnAddPair_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFirstLetter.Text) || string.IsNullOrEmpty(txtSecondLetter.Text))
                MessageBox.Show("You must enter 2 letters between A to Z!", "Invalid input");
            else if (!Regex.IsMatch(txtFirstLetter.Text.ToUpper(), "[A-Z]") || !Regex.IsMatch(txtSecondLetter.Text.ToUpper(), "[A-Z]"))
                MessageBox.Show("You must enter 2 letters between A to Z!", "Invalid input");
            else
            {
                try
                {
                    Pair.SetPair(ref pairs, txtFirstLetter.Text.ToUpper().ToCharArray()[0], txtSecondLetter.Text.ToUpper().ToCharArray()[0]);
                    lbxPlugBoardPairs.Items.Add(pairs[pairs.Count - 1].ToString());
                    txtFirstLetter.Clear();
                    txtSecondLetter.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnRemovePair_Click(object sender, RoutedEventArgs e)
        {
            if (lbxPlugBoardPairs.SelectedItem != null)
            {
                char[] selected = lbxPlugBoardPairs.SelectedItem.ToString().ToCharArray();
                Pair.RemovePair(ref pairs, selected[0], selected[1]);
                lbxPlugBoardPairs.Items.Remove(lbxPlugBoardPairs.SelectedItem);
            }
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key < Key.A || e.Key > Key.Z)
            {
                e.Handled = true;
                return;
            }

            char key = e.Key.ToString()[0];

            rightRotor.MoveUp(true);
            UpdateOffsetLabel();
            char output = GetPlugboardTrans(rightRotor.GetInput(GetPlugboardTrans(key)));
            txtOutput.Text += output;
            txtInput.CharacterCasing = CharacterCasing.Upper;
        }

        private char GetPlugboardTrans(char input)
        {
            if (pairs.Count > 0)
            {
                foreach (Pair p in pairs)
                {
                    if (input == p.FirstLetter)
                        return p.SecondLetter;
                    else if (input == p.SecondLetter)
                        return p.FirstLetter;
                }
            }
            return input;
        }

        private void ResetText()
        {
            txtInput.Clear();
            txtOutput.Clear();
        }

        private void DoAutoEncryption()
        {
            if (txtInput.Text.Length > 0)
            {
                for (int i = 0; i < txtInput.Text.Length; i++)
                {
                    char letter = txtInput.Text.ToUpper()[i];
                    if (letter >= 'A' && letter <= 'Z')
                    {
                        rightRotor.MoveUp(true);
                        char output = GetPlugboardTrans(rightRotor.GetInput(GetPlugboardTrans(letter)));
                        txtOutput.Text += output;
                    }

                    if (letter == ' ')
                        txtOutput.Text += " ";
                }
                UpdateOffsetLabel();
                txtInput.CharacterCasing = CharacterCasing.Upper;
            }
        }

        private void btnAutoEncDec_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.Clear();
            if (!Regex.IsMatch(txtInput.Text.ToUpper(), "[A-Z]"))
                MessageBox.Show("Invalid input text!", "Warning");
            else
                DoAutoEncryption();
        }

        private void txtInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                txtOutput.Text += " ";
        }

        private void btnLeftUpRing_Click(object sender, RoutedEventArgs e)
        {
            leftRotor.MoveRingUp();
            UpdateOffsetLabel();
            ResetText();
        }

        private void btnMiddleUpRing_Click(object sender, RoutedEventArgs e)
        {
            middleRotor.MoveRingUp();
            UpdateOffsetLabel();
            ResetText();
        }

        private void btnRightUpRing_Click(object sender, RoutedEventArgs e)
        {
            rightRotor.MoveRingUp();
            UpdateOffsetLabel();
            ResetText();
        }

        private void btnLeftDownRing_Click(object sender, RoutedEventArgs e)
        {
            leftRotor.MoveRingDown();
            UpdateOffsetLabel();
            ResetText();
        }

        private void btnMiddleDownRing_Click(object sender, RoutedEventArgs e)
        {
            middleRotor.MoveRingDown();
            UpdateOffsetLabel();
            ResetText();
        }

        private void btnRightDownRing_Click(object sender, RoutedEventArgs e)
        {
            rightRotor.MoveRingDown();
            UpdateOffsetLabel();
            ResetText();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ResetText();
        }

        private void btnRandomMessage_Click(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();
            int length = rand.Next(10, 50);
            string message = "";

            for (int i = 0; i < length; i++)
            {
                if (i > 0 && i % 4 == 0)
                    message += " ";

                char c = (char)rand.Next('A', 'Z');
                message += c;
            }

            txtInput.Text = message;
        }

        private void btnRandomSettings_Click(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();
            List<int> rotorsNumbers = new List<int>();

            while (rotorsNumbers.Count != 3)
            {
                int num = rand.Next(0, 4);
                if (!rotorsNumbers.Contains(num))
                    rotorsNumbers.Add(num);
            }

            rightRotor = rotors[rotorsNumbers[0]];
            middleRotor = rotors[rotorsNumbers[1]];
            leftRotor = rotors[rotorsNumbers[2]];

            cmbRightRotor.SelectedIndex = rotorsNumbers[0];
            cmbMiddleRotor.SelectedIndex = rotorsNumbers[1];
            cmbLeftRotor.SelectedIndex = rotorsNumbers[2];

            SetRotorsPosition();

            rightRotor.Offset = (char)rand.Next('A', 'Z');
            rightRotor.InnerRingSetting = (char)rand.Next('A', 'Z');
            middleRotor.Offset = (char)rand.Next('A', 'Z');
            middleRotor.InnerRingSetting = (char)rand.Next('A', 'Z');
            leftRotor.Offset = (char)rand.Next('A', 'Z');
            leftRotor.InnerRingSetting = (char)rand.Next('A', 'Z');

            UpdateOffsetLabel();

            int pairsCount = rand.Next(0, 10);

            pairs.Clear();
            lbxPlugBoardPairs.Items.Clear();

            while (pairs.Count < pairsCount)
            {
                char first = (char)rand.Next('A', 'Z');
                char second = (char)rand.Next('A', 'Z');
                if (first == second)
                    continue;

                try
                {
                    Pair.SetPair(ref pairs, first, second);
                }
                catch
                {
                    continue;
                }
            }

            foreach (Pair p in pairs)
                lbxPlugBoardPairs.Items.Add(p.ToString());

        }

        private void cmbLeftRotor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowHideAvilableRotors();
            leftRotor.Offset = 'A';
            leftRotor.InnerRingSetting = 'A';
            int selectedIndex = ((ComboBox)sender).SelectedIndex;
            leftRotor = rotors[selectedIndex];
            SetRotorsPosition();
            UpdateOffsetLabel();
            ResetText();
        }

        private void cmbMiddleRotor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowHideAvilableRotors();
            middleRotor.Offset = 'A';
            middleRotor.InnerRingSetting = 'A';
            int selectedIndex = ((ComboBox)sender).SelectedIndex;
            middleRotor = rotors[selectedIndex];
            SetRotorsPosition();
            UpdateOffsetLabel();
            ResetText();
        }

        private void cmbRightRotor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowHideAvilableRotors();
            rightRotor.Offset = 'A';
            rightRotor.InnerRingSetting = 'A';
            int selectedIndex = ((ComboBox)sender).SelectedIndex;
            rightRotor = rotors[selectedIndex];
            SetRotorsPosition();
            UpdateOffsetLabel();
            ResetText();
        }
    }
}

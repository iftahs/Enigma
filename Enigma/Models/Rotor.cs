using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Models
{
    class Rotor
    {
        public string Layout { get; set; }
        public char Offset { get; set; }
        public char InnerRingSetting { get; set; }
        public string Ab { get; set; }
        public Rotor NextRotor { get; set; }
        public Rotor PreviousRotor { get; set; }
        public char NotchPosition { get; set; }
        public char Input { get; set; }
        public int RotorNumber { get; set; }
        public bool Reflector { get; set; }

        public Rotor(string layout, char offset, char notchPosition, int rotorNumber, bool reflector, char innerRingSetting)
        {
            Layout = layout;
            Offset = offset;
            NotchPosition = notchPosition;
            RotorNumber = rotorNumber;
            Reflector = reflector;
            InnerRingSetting = innerRingSetting;
            Ab = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        }

        public void MoveUp()
        {
            if (Offset == 'Z')
                Offset = 'A';
            else
                Offset++;
        }

        public void MoveDown()
        {
            if (Offset == 'A')
                Offset = 'Z';
            else
                Offset--;
        }

        public void MoveRingUp()
        {
            if (InnerRingSetting == 'Z')
                InnerRingSetting = 'A';
            else
                InnerRingSetting++;
        }

        public void MoveRingDown()
        {
            if (InnerRingSetting == 'A')
                InnerRingSetting = 'Z';
            else
                InnerRingSetting--;
        }

        public char GetInput(char input)
        {
            if (Reflector)
                return Layout[Ab.IndexOf(input)];

            int inputIndex = Ab.IndexOf(input);
            int offsetIndex = Ab.IndexOf(Offset);
            int innerRingIndex = Ab.IndexOf(InnerRingSetting);

            int index = inputIndex + offsetIndex - innerRingIndex;
            if (index > 25) index = index - 26;
            if (index < 0) index = index + 26;

            index = Ab.IndexOf(Layout[index]) - offsetIndex + innerRingIndex;
            if (index > 25) index = index - 26;
            if (index < 0) index = index + 26;

            char returnedChar = NextRotor.GetInput(Ab[index]);
            int returnedCharIndex = Ab.IndexOf(returnedChar);

            index = returnedCharIndex + offsetIndex - innerRingIndex;
            if (index > 25) index = index - 26;
            if (index < 0) index = index + 26;

            int layoutIndex = Layout.IndexOf(Ab[index]);

            index = layoutIndex - offsetIndex + innerRingIndex;
            if (index > 25) index = index - 26;
            if (index < 0) index = index + 26;

            return Ab[index];
        }
    }
}

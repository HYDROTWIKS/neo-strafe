using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeoStrafe
{
    static class Program
    {
        // Windows API imports
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private const int VK_SPACE = 0x20;
        private const int VK_W = 0x57;
        private const int VK_A = 0x41;
        private const int VK_S = 0x53;
        private const int VK_D = 0x44;
        private const int VK_O = 0x4F;

        private const uint KEYEVENTF_KEYDOWN = 0x0000;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        private static bool isActive = false;
        private static Form1 mainForm;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainForm = new Form1();
            
            // Start the macro loop on a separate thread
            Task.Run(() => MacroLoop());
            
            Application.Run(mainForm);
        }

        static void MacroLoop()
        {
            while (mainForm != null && !mainForm.IsDisposed)
            {
                // Check for toggle key (O)
                if ((GetAsyncKeyState(VK_O) & 0x0001) != 0)
                {
                    isActive = !isActive;
                    mainForm.UpdateStatus(isActive);
                    Thread.Sleep(200); // Debounce
                }

                if (isActive)
                {
                    // Check each key individually
                    CheckAndSpamKey(VK_W, "W");
                    CheckAndSpamKey(VK_A, "A");
                    CheckAndSpamKey(VK_S, "S");
                    CheckAndSpamKey(VK_D, "D");
                    CheckAndSpamKey(VK_SPACE, "SPACE");
                }

                Thread.Sleep(10); // Main loop tick
            }
        }

        static void CheckAndSpamKey(int vKey, string keyName)
        {
            if ((GetAsyncKeyState(vKey) & 0x8000) != 0)
            {
                SpamKey(vKey);
            }
        }

        static void SpamKey(int vKey)
        {
            keybd_event((byte)vKey, 0, KEYEVENTF_KEYDOWN, UIntPtr.Zero);
            Thread.Sleep(15);
            keybd_event((byte)vKey, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            Thread.Sleep(15);
        }

        public static bool GetIsActive()
        {
            return isActive;
        }
    }

    public partial class Form1 : Form
    {
        private Label statusLabel;
        private Label titleLabel;
        private Label infoLabel;
        private Label keysLabel;

        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form settings
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 250);
            this.Name = "Form1";
            this.Text = "NEO STRAFE";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.Font = new System.Drawing.Font("Arial", 11F);

            // Title Label
            titleLabel = new Label();
            titleLabel.Text = "NEO STRAFE";
            titleLabel.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold);
            titleLabel.ForeColor = System.Drawing.Color.Cyan;
            titleLabel.Location = new System.Drawing.Point(20, 20);
            titleLabel.Size = new System.Drawing.Size(360, 40);
            titleLabel.TextAlign = System.Windows.Forms.ContentAlignment.MiddleCenter;
            this.Controls.Add(titleLabel);

            // Info Label
            infoLabel = new Label();
            infoLabel.Text = "Keyboard Macro Utility";
            infoLabel.Font = new System.Drawing.Font("Arial", 10F);
            infoLabel.ForeColor = System.Drawing.Color.Gray;
            infoLabel.Location = new System.Drawing.Point(20, 65);
            infoLabel.Size = new System.Drawing.Size(360, 25);
            infoLabel.TextAlign = System.Windows.Forms.ContentAlignment.MiddleCenter;
            this.Controls.Add(infoLabel);

            // Status Label
            statusLabel = new Label();
            statusLabel.Text = "Status: OFF";
            statusLabel.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            statusLabel.ForeColor = System.Drawing.Color.Red;
            statusLabel.Location = new System.Drawing.Point(20, 100);
            statusLabel.Size = new System.Drawing.Size(360, 35);
            statusLabel.TextAlign = System.Windows.Forms.ContentAlignment.MiddleCenter;
            this.Controls.Add(statusLabel);

            // Keys Label
            keysLabel = new Label();
            keysLabel.Text = "Toggle: O  |  Keys: W A S D SPACE  |  Exit: ESC";
            keysLabel.Font = new System.Drawing.Font("Arial", 10F);
            keysLabel.ForeColor = System.Drawing.Color.LimeGreen;
            keysLabel.Location = new System.Drawing.Point(20, 150);
            keysLabel.Size = new System.Drawing.Size(360, 50);
            keysLabel.TextAlign = System.Windows.Forms.ContentAlignment.MiddleCenter;
            this.Controls.Add(keysLabel);

            // Info text
            Label infoText = new Label();
            infoText.Text = "Hold W, A, S, D, or SPACE to spam press them";
            infoText.Font = new System.Drawing.Font("Arial", 9F);
            infoText.ForeColor = System.Drawing.Color.White;
            infoText.Location = new System.Drawing.Point(20, 210);
            infoText.Size = new System.Drawing.Size(360, 20);
            infoText.TextAlign = System.Windows.Forms.ContentAlignment.MiddleCenter;
            this.Controls.Add(infoText);

            this.ResumeLayout(false);
        }

        public void UpdateStatus(bool isActive)
        {
            if (statusLabel.InvokeRequired)
            {
                statusLabel.Invoke(new Action(() => UpdateStatus(isActive)));
                return;
            }

            if (isActive)
            {
                statusLabel.Text = "Status: ON";
                statusLabel.ForeColor = System.Drawing.Color.LimeGreen;
            }
            else
            {
                statusLabel.Text = "Status: OFF";
                statusLabel.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}

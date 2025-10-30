namespace SistemaAlquilerAutos.UI.Forms
{
    partial class FormClientes
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Gestión de Clientes";
            this.Size = new Size(900, 600);

            // Panel superior con controles
            Panel panelSuperior = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                Padding = new Padding(10)
            };

            Label lblBuscar = new Label
            {
                Text = "Buscar:",
                Location = new Point(10, 18),
                AutoSize = true
            };

            txtBuscar = new TextBox
            {
                Location = new Point(70, 15),
                Width = 250
            };

            btnBuscar = new Button
            {
                Text = "Buscar",
                Location = new Point(330, 14),
                Width = 80
            };
            btnBuscar.Click += BtnBuscar_Click;

            btnNuevo = new Button
            {
                Text = "Nuevo Cliente",
                Location = new Point(450, 14),
                Width = 120
            };
            btnNuevo.Click += BtnNuevo_Click;

            btnEditar = new Button
            {
                Text = "Editar",
                Location = new Point(580, 14),
                Width = 80
            };
            btnEditar.Click += BtnEditar_Click;

            btnEliminar = new Button
            {
                Text = "Eliminar",
                Location = new Point(670, 14),
                Width = 80
            };
            btnEliminar.Click += BtnEliminar_Click;

            panelSuperior.Controls.Add(lblBuscar);
            panelSuperior.Controls.Add(txtBuscar);
            panelSuperior.Controls.Add(btnBuscar);
            panelSuperior.Controls.Add(btnNuevo);
            panelSuperior.Controls.Add(btnEditar);
            panelSuperior.Controls.Add(btnEliminar);

            // DataGridView
            dgvClientes = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.Add(dgvClientes);
            this.Controls.Add(panelSuperior);

            this.ResumeLayout(false);
        }
    }
}

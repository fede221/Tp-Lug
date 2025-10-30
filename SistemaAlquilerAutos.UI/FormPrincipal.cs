namespace SistemaAlquilerAutos.UI
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
            this.Load += FormPrincipal_Load;
        }

        private void FormPrincipal_Load(object? sender, EventArgs e)
        {
            CrearMenu();
        }

        private void CrearMenu()
        {
            // MenuStrip principal
            MenuStrip menuStrip = new MenuStrip();

            // Menú Archivo
            ToolStripMenuItem menuArchivo = new ToolStripMenuItem("&Archivo");
            ToolStripMenuItem menuSalir = new ToolStripMenuItem("&Salir", null, MenuSalir_Click);
            menuSalir.ShortcutKeys = Keys.Alt | Keys.F4;
            menuArchivo.DropDownItems.Add(menuSalir);

            // Menú Gestión
            ToolStripMenuItem menuGestion = new ToolStripMenuItem("&Gestión");
            ToolStripMenuItem menuClientes = new ToolStripMenuItem("&Clientes", null, MenuClientes_Click);
            menuClientes.ShortcutKeys = Keys.Control | Keys.C;
            ToolStripMenuItem menuVehiculos = new ToolStripMenuItem("&Vehículos", null, MenuVehiculos_Click);
            menuVehiculos.ShortcutKeys = Keys.Control | Keys.V;
            ToolStripMenuItem menuCategorias = new ToolStripMenuItem("C&ategorías", null, MenuCategorias_Click);
            ToolStripMenuItem menuSucursales = new ToolStripMenuItem("&Sucursales", null, MenuSucursales_Click);

            menuGestion.DropDownItems.Add(menuClientes);
            menuGestion.DropDownItems.Add(menuVehiculos);
            menuGestion.DropDownItems.Add(new ToolStripSeparator());
            menuGestion.DropDownItems.Add(menuCategorias);
            menuGestion.DropDownItems.Add(menuSucursales);

            // Menú Alquileres
            ToolStripMenuItem menuAlquileres = new ToolStripMenuItem("&Alquileres");
            ToolStripMenuItem menuNuevoAlquiler = new ToolStripMenuItem("&Nuevo Alquiler", null, MenuNuevoAlquiler_Click);
            menuNuevoAlquiler.ShortcutKeys = Keys.Control | Keys.N;
            ToolStripMenuItem menuAlquileresActivos = new ToolStripMenuItem("Alquileres &Activos", null, MenuAlquileresActivos_Click);
            ToolStripMenuItem menuHistorialAlquileres = new ToolStripMenuItem("&Historial", null, MenuHistorialAlquileres_Click);
            ToolStripMenuItem menuDevolverVehiculo = new ToolStripMenuItem("&Devolver Vehículo", null, MenuDevolverVehiculo_Click);

            menuAlquileres.DropDownItems.Add(menuNuevoAlquiler);
            menuAlquileres.DropDownItems.Add(new ToolStripSeparator());
            menuAlquileres.DropDownItems.Add(menuAlquileresActivos);
            menuAlquileres.DropDownItems.Add(menuHistorialAlquileres);
            menuAlquileres.DropDownItems.Add(new ToolStripSeparator());
            menuAlquileres.DropDownItems.Add(menuDevolverVehiculo);

            // Menú Ayuda
            ToolStripMenuItem menuAyuda = new ToolStripMenuItem("&Ayuda");
            ToolStripMenuItem menuAcercaDe = new ToolStripMenuItem("&Acerca de...", null, MenuAcercaDe_Click);
            menuAyuda.DropDownItems.Add(menuAcercaDe);

            // Agregar menús al MenuStrip
            menuStrip.Items.Add(menuArchivo);
            menuStrip.Items.Add(menuGestion);
            menuStrip.Items.Add(menuAlquileres);
            menuStrip.Items.Add(menuAyuda);

            // StatusStrip
            StatusStrip statusStrip = new StatusStrip();
            ToolStripStatusLabel statusLabel = new ToolStripStatusLabel("Sistema de Alquiler de Autos v1.0");
            statusStrip.Items.Add(statusLabel);

            // Agregar controles al formulario
            this.Controls.Add(statusStrip);
            this.Controls.Add(menuStrip);
            this.MainMenuStrip = menuStrip;
        }

        // Event Handlers
        private void MenuSalir_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MenuClientes_Click(object? sender, EventArgs e)
        {
            AbrirFormularioMdi(new Forms.FormClientes());
        }

        private void MenuVehiculos_Click(object? sender, EventArgs e)
        {
            AbrirFormularioMdi(new Forms.FormVehiculos());
        }

        private void MenuCategorias_Click(object? sender, EventArgs e)
        {
            AbrirFormularioMdi(new Forms.FormCategorias());
        }

        private void MenuSucursales_Click(object? sender, EventArgs e)
        {
            AbrirFormularioMdi(new Forms.FormSucursales());
        }

        private void MenuNuevoAlquiler_Click(object? sender, EventArgs e)
        {
            AbrirFormularioMdi(new Forms.FormNuevoAlquiler());
        }

        private void MenuAlquileresActivos_Click(object? sender, EventArgs e)
        {
            AbrirFormularioMdi(new Forms.FormAlquileresActivos());
        }

        private void MenuHistorialAlquileres_Click(object? sender, EventArgs e)
        {
            AbrirFormularioMdi(new Forms.FormHistorialAlquileres());
        }

        private void MenuDevolverVehiculo_Click(object? sender, EventArgs e)
        {
            AbrirFormularioMdi(new Forms.FormDevolverVehiculo());
        }

        private void MenuAcercaDe_Click(object? sender, EventArgs e)
        {
            MessageBox.Show(
                "Sistema de Alquiler de Autos v1.0\n\n" +
                "Desarrollado con arquitectura en capas\n" +
                "- Capa de Presentación (UI)\n" +
                "- Capa de Lógica de Negocio (BLL)\n" +
                "- Capa de Acceso a Datos (DAL)\n" +
                "- Capa de Entidades (Entity)\n" +
                "- Capa de Mapeo (Mapper)\n\n" +
                "© 2024",
                "Acerca de",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void AbrirFormularioMdi(Form formulario)
        {
            // Cerrar formularios del mismo tipo que ya estén abiertos
            foreach (Form form in this.MdiChildren)
            {
                if (form.GetType() == formulario.GetType())
                {
                    form.Activate();
                    return;
                }
            }

            formulario.MdiParent = this;
            formulario.WindowState = FormWindowState.Maximized;
            formulario.Show();
        }
    }
}

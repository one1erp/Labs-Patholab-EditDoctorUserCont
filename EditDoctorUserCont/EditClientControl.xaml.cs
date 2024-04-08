using LSSERVICEPROVIDERLib;
using Patholab_DAL_V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Patholab_XmlService;
using System.Windows.Forms;

namespace EditDoctorUserCont
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class EditDoctorControl : System.Windows.Window
    {
        private INautilusServiceProvider _sp;
        private SUPPLIER_USER supplier;
        public event Action<SUPPLIER_USER> DoctorEdited;


        public EditDoctorControl(SUPPLIER_USER s, INautilusServiceProvider sp)
        {
            InitializeComponent();

            _sp = sp;
            supplier = s;
            textBoxID.Text = supplier.U_ID_NBR;
            textBoxLicenseNum.Text = supplier.U_LICENSE_NBR;
            textBoxFullName.Text = supplier.U_LAST_NAME;
            textBoxDegree.Text = supplier.U_DEGREE;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            UpdateStaticEntity upcl = new UpdateStaticEntity(_sp);
            upcl.Login("SUPPLIER", "Supplier", FindBy.Id, supplier.SUPPLIER_ID.ToString());
            upcl.AddProperties("U_LAST_NAME", textBoxFullName.Text);
            upcl.AddProperties("U_DEGREE", textBoxDegree.Text);
            upcl.AddProperties("U_ID_NBR", textBoxID.Text);
            upcl.AddProperties("U_LICENSE_NBR", textBoxLicenseNum.Text);


            var s = upcl.ProcssXml();
            if (!s)
            {

                MessageBox.Show(string.Format("Error on Edit Doctor  {0}", upcl.ErrorResponse), "Edit Doctor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("הרופא עודכן במערכת.");

                supplier.U_ID_NBR = upcl.GetValueByTagName("U_ID_NBR");
                supplier.U_LICENSE_NBR = upcl.GetValueByTagName("U_LICENSE_NBR");
                supplier.U_LAST_NAME = upcl.GetValueByTagName("U_LAST_NAME");
                supplier.U_DEGREE = upcl.GetValueByTagName("U_DEGREE");
                if (DoctorEdited != null)
                    DoctorEdited(supplier);

                this.Close();

            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            var dg = MessageBox.Show("האם אתה בטוח שברצונך לצאת?", "Nautilus - עדכון רופא", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dg == System.Windows.Forms.DialogResult.Yes)
            {
                this.Close();
            }

        }
    }
}

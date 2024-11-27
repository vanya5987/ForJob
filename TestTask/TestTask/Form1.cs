using DirectShowLib;
using Emgu.CV;
using Emgu.CV.Structure;


namespace TestTask
{
    public partial class TestTask : Form
    {
        private VideoCapture _capture;
        private DsDevice[] _cams;
        private int _selectedCameraId = 0;

        public TestTask()
        {
            InitializeComponent();
        }

        private void TestTask_Load(object sender, EventArgs e)
        {
            _cams = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            for (int i = 0; i < _cams.Length; i++)
                CameraChoice.Items.Add(_cams[i].Name);
        }

        private void CameraChoiceSelectedIndexChange(object sender, EventArgs e)
        {
            _selectedCameraId = CameraChoice.SelectedIndex;
        }

        private void Viewer_Click(object sender, EventArgs e)
        {
            if (_cams.Length == 0)
                throw new IndexOutOfRangeException("Камеры не найдены...");

            if (CameraChoice.SelectedItem == null)
                throw new FormatException("Камера не выбрана...");

            _capture = new VideoCapture(_selectedCameraId);

            _capture.ImageGrabbed += Capture_ImageGrabbed;

            _capture.Start();
        }

        private void Capture_ImageGrabbed(object? sender, EventArgs e)
        {
            Mat mat = new Mat();

            _capture.Retrieve(mat);

            FramePictures.Image = mat.ToImage<Bgr, byte>().Flip(Emgu.CV.CvEnum.FlipType.Horizontal).Bitmap;
        }
    }
}

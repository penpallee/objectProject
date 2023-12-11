

namespace MachinVisionProgram {
    public static class ImageProcess {
        public static async Task<(double, double)> GetAlignPosition(string imageUrl) {
            return await Task.Run(() => {
                //image process for calculate align position
                //return (x, y);
                //return (0, 0);
                
                return (0, 0);
            });
        }

        public static async Task<byte[]> LaneImageCalculate(byte[] image) {
                return await Task.Run(() => { return new byte[5]; });
        }

        public static async Task<byte[]> DimensionCameraCalculate(byte[] image) {
                return await Task.Run(() => { return new byte[5]; });
        }

        public static bool FinalDecision() {
            Console.WriteLine("최종적으로 합격입니다.");
            return true;
        } 
    }
}
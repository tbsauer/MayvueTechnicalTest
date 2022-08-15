using MotionPicturesCore.Models;

namespace MotionPicturesCore.Interfaces
{
    public interface IMotionPictureService
    {
        public Task<IEnumerable<MotionPicture>> GetMotionPictures();
        public Task<MotionPicture> GetMotionPicture(int id);
        public Task<MotionPicture> CreateMotionPicture(MotionPictureAddRequest movie);
        public Task UpdateMotionPicture(int id, MotionPictureUpdateRequest movie);
        public Task DeleteMotionPicture(int id);
    }
}

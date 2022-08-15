namespace MotionPicturesCore.Models
{
    public class MotionPicture
    {
        public int  Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public int ReleaseYear { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public int CreatedBy { get; set; }
    }
   
}

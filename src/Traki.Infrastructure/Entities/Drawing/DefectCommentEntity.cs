﻿namespace Traki.Infrastructure.Entities.Drawing
{
    public class DefectCommentEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string ImageName { get; set; }
        public string Date { get; set; }
        public int DefectId { get; set; }
        public DefectEntity Defect { get; set; }
        public int AuthorId { get; set; }
        public UserEntity Author { get; set; }
    }
}

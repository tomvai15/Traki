﻿namespace Traki.Domain.Models
{
    public class Protocol
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSigned { get; set; }
        public string CreationDate { get; set; }
        public bool IsCompleted { get; set; }
        public int? SignerId { get; set; }
        public User? Signer { get; set; }
        public string? ReportName { get; set; }
        public string? EnvelopeId { get; set; }
        public bool IsTemplate { get; set; }
        public int? ProductId { get; set; }
    }
}

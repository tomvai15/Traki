﻿using Traki.Infrastructure.Entities.Drawing;

namespace Traki.Api.Contracts.Drawing.Defect
{
    public class CreateStatusChangeRequest
    {
        public StatusChangeDto StatusChange { get; set; }
    }
}

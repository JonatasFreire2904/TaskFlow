﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.UserDtos
{
    public record UserLoginDto(string Email, string Password);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSitoPanel.src.dtos.users;

public record CreateUserSessionDTO(string? id, string? name, string? tenant_id);
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSitoPanel.src.dtos.users;

public record ReadUserSessionDTO(string id, string name, string tenant_id, string timeStamp, string Token);
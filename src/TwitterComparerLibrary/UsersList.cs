﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TwitterComparerLibrary
{
    public class UsersList
    {
        [JsonProperty("users")]
        public List<User> Users { get; set; }
    }
}

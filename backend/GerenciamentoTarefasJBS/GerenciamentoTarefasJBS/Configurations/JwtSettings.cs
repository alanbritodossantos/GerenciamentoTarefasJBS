﻿namespace GerenciamentoTarefasJBS.Configurations
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
    }
}

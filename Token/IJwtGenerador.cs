using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using preparacion_pt_bdv.models;

namespace preparacion_pt_bdv.Token
{
    public interface IJwtGenerador
    {
        //Definición del método CrearToken que recibe un objeto Usuario y retorna un string
        string CrearToken(Usuario usuario);   
    }
}
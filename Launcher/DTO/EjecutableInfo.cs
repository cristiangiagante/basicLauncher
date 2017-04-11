namespace Launcher
{
    public abstract class EjecutableInfo
    {
        public bool EjecucionAutomaticaAlActualizar { get; set; } = true;
        public bool VerificarIntegridadAlEjecutar { get; set; } = true;
        public bool DetenerEjecucionAlVerificarIntegridad { get; set; } = false;
        public bool ActualizarAutomaticamente { get; set; } = false;
        public string Argumentos { get; set; } = "";
    }
}
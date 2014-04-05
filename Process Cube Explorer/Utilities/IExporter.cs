using System;

namespace pgmpm.MainV2.Utilities
{
    /// <summary>
    /// Interface for exporter
    /// </summary>
    /// <autor>Andrej Albrecht</autor>
    public interface IExporter
    {
        bool export(String filename);
    }
}

public class IED
{
    public string Id { get; private set; }
    private float corrente;
    private readonly Protecao50 protecao50;
    private readonly Protecao51 protecao51;

    public IED(string id)
    {
        Id = id;
        protecao50 = new Protecao50(id);
        protecao51 = new Protecao51(id);
    }

    public float Corrente
    {
        get { return corrente; }
        set
        {
            if (corrente != value)
            {
                corrente = value;
                VerificarProtecoes();
            }
        }
    }

    // Método para iniciar o monitoramento em uma thread
    public void IniciarMonitoramento()
    {
        new Thread(() =>
        {
            while (true)
            {
                // Monitoramento contínuo com um intervalo de tempo
                VerificarProtecoes();
                Thread.Sleep(1000); // Intervalo de verificação
            }
        }).Start();
    }

    private void VerificarProtecoes()
    {
        Console.WriteLine($"IED {Id}: Verificando proteções para corrente = {corrente} A");

        // Verifica proteção 50
        if (protecao50.VerificarSobrecorrente(corrente))
        {
            protecao50.EmitirAlerta();
        }

        // Verifica proteção 51
        if (protecao51.VerificarSobrecorrente(corrente))
        {
            protecao51.EmitirAlerta();
        }
    }
}

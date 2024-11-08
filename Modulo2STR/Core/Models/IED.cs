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

    public void IniciarMonitoramento()
    {
        new Thread(() =>
        {
            while (true)
            {

                VerificarProtecoes();
                Thread.Sleep(1000); 
            }
        }).Start();
    }

    private void VerificarProtecoes()
    {
        Console.WriteLine($"IED {Id}: Verificando proteções para corrente = {corrente} A");

        
        if (protecao50.verificarSobrecorrente(corrente))
        {
            protecao50.EmitirAlerta();
        }

        
        if (protecao51.verificarSobrecorrente(corrente))
        {
            protecao51.EmitirAlerta();
        }
    }
}

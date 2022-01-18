namespace kv.core;

public class Feedback
{
    private TextWriter stdout;

    public Feedback(TextWriter stdout)
    {
        this.stdout = stdout;
    }

    public void PrintMessage(string msg)
    {
        stdout.WriteLine(msg);
    }
}
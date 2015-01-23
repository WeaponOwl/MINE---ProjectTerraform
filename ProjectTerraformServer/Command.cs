namespace ProjectTerraform
{
    enum commands
    {
        move,
        moveandbuild,
        patrol,
        tradergoin,
        tradergoaway,
        tradegodown,
        tradegoup,
        humangoin,
        humangoaway,
        humangodown,
        humangoup,
        patroltobeacon,
        piraterob,
        pirateaway
    }

    class Command
    {
        public commands message;
        public int x;
        public int y;
        public int id;

        public Command(commands text, int x, int y)
        {
            message = text;
            this.x = x;
            this.y = y;
        }
        public Command(commands text, int id)
        {
            message = text;
            this.id = id;
        }
    }
}

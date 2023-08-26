namespace BankAPI.Models.Messages
{
    public class BankIdMessageMapper
    {

        private Dictionary<string, BankIdMessages> _serverToClientMessage;
        public BankIdMessageMapper()
        {
            _serverToClientMessage = new Dictionary<string, BankIdMessages>
            {
                {"RFA1",BankIdMessages.RFA1 },
                {"RFA2",BankIdMessages.RFA2 },
                {"RFA3",BankIdMessages.RFA3 },
                {"RFA4",BankIdMessages.RFA4 },
                {"RFA5",BankIdMessages.RFA5 },
                {"RFA6",BankIdMessages.RFA6 },
                {"RFA8",BankIdMessages.RFA8 },
                {"RFA9",BankIdMessages.RFA9 },
                {"RFA13",BankIdMessages.RFA13 },
                {"RFA14(A)",BankIdMessages.RFA14A },
                {"RFA14(B)",BankIdMessages.RFA14B },
                {"RFA15(A)",BankIdMessages.RFA15A },
                {"RFA15(B)",BankIdMessages.RFA15B },
                {"RFA16",BankIdMessages.RFA16 },
                {"RFA17(A)",BankIdMessages.RFA17A },
                {"RPA17(B)",BankIdMessages.RPA17B },
                {"RFA18",BankIdMessages.RFA18 },
                {"RFA19",BankIdMessages.RFA19 },
                {"RFA20",BankIdMessages.RFA20 },
                {"RFA21",BankIdMessages.RFA21 },
                {"RFA22",BankIdMessages.RFA22 },
                {"RFA23",BankIdMessages.RFA23},



            };
        }
    }
}

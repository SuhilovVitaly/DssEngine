﻿using DeepSpaceSaga.Common.Abstractions.Dto.Ui;

namespace DeepSpaceSaga.Common.Abstractions.UI;

public interface IGameEventsService
{
    void UpdateGameData(GameSessionDto session);
}

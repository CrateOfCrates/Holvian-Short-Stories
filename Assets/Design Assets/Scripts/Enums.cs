public enum WhichTeam {player, enemy, undec};
public enum TokenType {delay, singleAttack, multiAttack, block, dodge, applyBuff}
public enum Target {self, target, entireSide, allActive, none}
public enum BattleState {startBattle, playerSelect, selectMove, enemySelect, execute, victory, defeat};

public enum StatusEffectDecayRate {fractional, subtractional};
public enum StatusEffectPartType {afterAnyToken, endOfTurn};
public enum TriggerTime {before, after}

public enum StatusEffectType {damage, statChange, resource, none}

public enum StatChangeType {damageChangeMultAdd}

//public enum Status {burn}




FSM-based Unit Testing ​of the C# implementation

Relevance of Coursework:
1. FSM methods allow to generate tests with guaranteed fault coverage;​
2. Unit testing libraries provide tools for implementating testing.

Aim - development of an automated translator of abstract tests into unit tests.

Objectives:​
1. Unit testing tools introduction;​
2. Software development tools selection​;
3. Test translator development​;
4. The developed software debugging using a demo example.

A finite state machine is defined as an ordered quintuplet of sets:​
  A = {S, I, O, T, s}, where: ​
    S — the set of states of the FSM;
    I — input alphabet;
    O — output alphabet; ​
    T ⊆ S * I * O * S — transition relation;​
    s — initial state of the machine.

The fault model of a finite state machine is defined as a triple of sets: ​
  FM = <S, ≅, FDm>, where: ​
    S – fsm specification, ​
    ≅ – equivalence relation, ​
    FDm is a fault domain contains a set of finite state machines with the same input and output alphabets as the specification and no more than m states. ​

Methods for fsm-based test derevations are the transition tour and the W-method. ​

Unit testing is a software method for testing modules or components of a program. ​
One of the most famous tools (libraries) for unit testing C# applications (as one of the languages included in the .NET SDK) is MSTest.

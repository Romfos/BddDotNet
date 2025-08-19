Feature: Scenario Outline feature

Scenario Outline: scenario outline with 2 examples
    Given this is given step with argument '<book>'
    Then this is then step with table:
    | book   | price   |
    | <book> | <price> |
    | static | 99      |

    Examples: 
    | book      | price |
    | sharpener | 30    |

    Examples: 
    | book   | price |
    | pencil | 15    |
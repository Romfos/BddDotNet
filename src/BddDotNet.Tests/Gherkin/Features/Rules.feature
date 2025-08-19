Feature: Rules feature

Rule: this is example of rule

Scenario: scenario from rule
    And this is simple given step

Scenario Outline: scenario outline from rule
    Given this is given step with argument '<book>'

    Examples: 
    | book      |
    | sharpener |
    | pencil    | 
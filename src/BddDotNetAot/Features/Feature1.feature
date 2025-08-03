@feature-tag
Feature: DemoApp feature

A short summary of the feature

@scenario-tag-1
Scenario: this is scenario with simple steps
    Given this is given step
    When this is when step
    Then this is then step

@scenario-tag-2
Scenario: this is scenario for step with arguments
    Given given step with argument 'abcd'
    Then this is step with table:
    | book      | price |
    | sharpener | 30    |
    | pencil    | 15    |
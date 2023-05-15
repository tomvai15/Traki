 
Feature: View protocol

Scenario: Open protocol tab displays protocols
    Given I have logged in as project manager
    When I press on Templates tab
    Then protocols should be displayed
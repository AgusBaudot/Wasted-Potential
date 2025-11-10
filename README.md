# Industrial TD.

## Brief intro.

Industrial TD is a tower defense game with deck-building elements and an emphasis on strategy and adaptability.
You play as the CEO of a multinational corporation expanding operations into a forest.
Your goal: defend company territory from the forest’s inhabitants — rabbits, deer, bears, and more — using industrialized defense towers.

Gameplay focuses on:

- Strategic tower placement

- Dynamic adaptation through cards

- System modularity and extensibility

## Used design patterns.

1. Singleton
2. Factory
3. Strategy
4. State
5. Flyweight
6. Pool
7. Abstract Factory
8. Type Object
9. Command
10. Observer
11. Facade
12. Service Locator

## Unused design patterns.

1. Event queue
2. Memento

## Used design patterns explanation.

#### 1. Singleton.

Used sparingly:

- GridManager.cs: This was one of the first scripts ever, and it's a Singleton just because originally the task said we had to implement a few. Every other potential singleton was treated as a service and registered themselves in the Service Locator.
- GameManager.cs: This is a singleton not for its access, but for the need to only have one Instance at a time, because it's the only object that uses the DontDestroyOnLoad.

#### 2. Factory.

Initial pattern for creating basic enemies (e.g., bears).
Centralizes prefab instantiation logic through a Create method.

#### 3. Strategy.

Implements multiple targeting priorities for towers:
- First
- Last
- Closest
- Weakest
Each tower uses its assigned ITargetingStrategy to pick an enemy from its candidates. 
Future versions could allow the player to swap targeting logic dynamically.

#### 4. State.

Manager transitions between game states:
- Menu state: controls the main menu and transitions to gameplay.
- Playing state: subdivided into sub-states
  1. Show cards
  2. Wait for player to place defenses.
  3. Start wave
- GameOver state: not yet implemented, but system is extendable.

#### 5. Flyweight.

Shared attributes (damage, health, cost, speed, etc.) are stored in ScriptableObjects, minimizing memory duplication and simplifying balancing.

#### 6. Pool.

Implemented using Unity's built-in ObjectPool<T>.
Each factory integrates pooling logic:
- OnGet(): initialize object.
- OnRelease(): reset object.
Pools exist for different enemy types for modularity and reuse.

#### 7. Abstract Factory.

Evolved from the simple factory as new enemies were added.
Factories implement a shared interface and are retrieved via a FactoryProvider, which mediates between the spawner and concrete factories.

#### 8. Type Object.

Each tower and enemy type is defined through a ScriptableObject asset (CardData, TowerData, etc.), serving as a flexible object-type system.

#### 9. Command.

BuildTowerCommand encapsulates the multi-step tower construction process:
1. Spend resources.
2. Instantiate tower.
3. Update grid buildability.

This abstraction makes it possible to:
- Implement undo/rollback logic.
- Prevent concurrency issues (e.g., two towers on the same tile).
- Scale toward multyplayer or async gameplay.

#### 10. Observer.

Used for decoupled communication.
Examples:
- WaveManager -> CardManager (signal: every 3 rounds grant a new card).
- Enemy -> EnemyManager (signal: on enemy removed unregister enemy).

#### 11. Facade.

The TowerPlacementFacade wraps complex building logic (validation, resource check, grid updates, and command execution) into a simple API:

``TryPlaceTower(CardData card, Vector2Int gridPosition)``

#### 12. Service Locator.

Registers and provides global access to essential services:
- ResourceManager
- UpdateManager
- WaveManager
- CardManager
- EnemyManager
Used as a lightweight dependency provider, replacing excessive singletons.

## Architecture Overview.

### Core systems.

| System                   | Responsibility                                    |
| ------------------------ | ------------------------------------------------- |
| **GridManager**          | Handles grid-to-world conversions and tile states |
| **EnemyManager**         | Tracks and queries enemies                        |
| **TowerManager**         | Tracks all placed towers and manages UI events    |
| **ResourceManager**      | Handles money/resource logic                      |
| **CardManager**          | Controls available cards and upgrades             |
| **TowerPlacementFacade** | Simplifies tower placement logic                  |
| **FactoryProvider**      | Provides concrete factory references              |
| **ServiceLocator**       | Registers global services                         |

### Data types

| Asset         | Purpose                                              |
| ------------- | ---------------------------------------------------- |
| **CardData**  | Holds cost, image, rarity, and prefab reference      |
| **TowerData** | Holds damage, range, fire rate, projectile reference |
| **EnemyData** | Holds speed, health, and reward values               |


## Unused design patterns explanation.

#### 1. Event Queue.

Event Queue could be useful if we had a series of actions (or events) to store and execute in a certain order at given time. In our game, we don't have anything of that sort.

#### 2. Memento.

Memento could serve us if we wanted to store the state of an object in order to restore it later. Since we don't have any need as such in our game, we didn't implement this pattern.

## UML

Link to our UML:
[![](https://img.plantuml.biz/plantuml/svg/lHjTKoEvyNshzZ-e_70nMUFQIzu8eGuCoxgo71GskvMdb1WBCzdno3SpXlLH-z_JBSr7jpxiONC93s1BtUfFjRfRufUoaaMrMMLlt_n9pBwckLZBGgvKfOhoxPiI1intaMIoB4-hgaZlDvMQ9ZgVfRyhyTw3EPF9rsMXD_bYeZDTY4_fyh4wopQAGfQmlDBLTgqcGVX_gYpJpnGZpG7bGIOg27zPA9LJy3L0oMKKU4jXLRvPXG5rjb3vhLuqxA2E3WyFnRbwI7CbRYoHKZm0niKtO32N6KokLGvuIGep0FtsJSsC6Cos8FXg85xUlX7mO_GbJfCgrJaTcPwdvLgNyZvJT7YcYbHcwUyutbf271_BvlF9oTittproOv0eKuP8wP2UNe3egf2LBlPXZYzL3lGJPuN1UMhcPR4zdlz3RKTY_bc2iKy6GfQYcsGu3LcGutXk8SaG0_qeoyUPgXgm-Yi3-PIMxJn-vgHkbLnSvzcMGh51BlXTdbRRYtm9vdNDj5vdQI9TMqqaiir7yYTP-YENu5vhEdm9gDVt_mO5qz4hdUipzIYVKhqfwCGj0CuBcPVWRIlc5qcHhYjKdR_QR5sabV97Rj552yK811V_30lguY_JlF97_qg7liYq-g2BcOBGi2YZMXrVFAcyilxbkkEPB9MPFfSLKod1iSDjD13J6ss-NE2lZp9VP8rG0PXlQRK7P08s_Xg3cIdS3R7PSrNwyvR_XdudMqmxDrfdxXQpLiMPne6xaVXAimGsx9NCOM85P9r5oMpXZTi138lYavOB2wiNAavidcPg9TUkGUjXTmZj75_hRkgwnQQgd02v0Yo-mldbUFuCXq2QBntqysip0O4IZhegJJ1Aqg3wsmPYDTiY5nv_ywkRGhl2gM_LbIeVOURkycvg49e3OcOeYOygMwj2J13UEmpifAio2F9FQQ8-wGIZCc6w0SEV5pluNRo3gB1CyJG5rnZEHMcn3uAWboQE3W_4aPW70UxoOiTgt-qVXy7n-8I5UIfBBU5-8O0p6qSDSpIa-e0OPuOFcPQLU8QFYE26dWRndVWW5sgQ3qa05ibo92nwbQvK-rcBlukV7mvSvEjDrHVx_GDNL2EzqP3rGUOZqoPqk8xHJRI-GSBCHAzM466e9jz1yg4II02C-kvDM22C4EIxzKAQauAZpo7erJfTgAoISv2D2-EjD9V5KWMM-gABR76ZonGz0hYfJmhn8W2yMSs2JilJB7rIDSiX23npj-SIT_iGQ0kvWWoj6f6JKvHmIhekwZ5xaQlLzeD4JhQStKcX9EYjuHAohf43heiKmX8grWnJv5k8zn0gXcO2uYNyvepmbT0H1ZW2YU7Op0mY_69azrKxXI1f4i0-_78-IrKD7TPQ6iWMMsAdnI6-pYh86DNIqQGz72tGa6PuPkaJaO3Jfk1z2ZYUwsS8YnN-TXJRSESJYsel1ck8b6Fp9wPeCnbqZ7cnhNrZ8ei57jeWGR4O2QRwBZiIoo95lny9lQaQ0Vbsv4v3TU38JfaoeY3vSSF780hcI4nYpKGNokmQSp8f9-1q4w60Op5ukC5PCpvaZeO9Lc3OPaHimWzDiyrzZ19VpSIm066ojBSuIcXMd-YyAh2gAPmuy2ZpPSrYBN6kdiq7npgXfSmMCeCHFskHwjFJ8Ruf2anm3TcZLkD-FBpEBMN3x090RUruR35E8emsD2cNRXrIfu82SXge39mgX79xAFw5nt6BS2HQEYoThBcWQnyUddZ0W3z8p3uhsHxjE6FGwMgTcNIszEKR1MqpYXvwFRHEX61iT9f4_bTcDCJ_5UJG5emopjoy9j51chK3WeJWRZAvDN6lvYIkIGOPPQOsS59hfjcHHztkf0gaQvhZELDFAkFRXT55bddw600Q_3mG6aBMuFru31-dX0j7qzxQWxlfUFQedp5IGkwqKo4j7DT680YXGW2mSzX9zurdnksICK4vnA1Uf1oJ5GtRcpBCCxRgliWdjPFdxxmICLMpQ4Y98TlPvK6mGzM2csgpnMM5oaSbi-gnTzo89NDkpamNZQdHtM9tqrUmWJQO9TWl7D8qqemSU7koMph6J0YAbfIiiCIqvJMXLjby1toYQOJPR8eA13LHdHPDyriy67XsQDFw0YSm7J5zCBf8BInZ3VVWuQ4sU_3F0pVjydSYG-vZjNhhzZGSZoNxJH84n7r33froSmysJ-_TQCAH87CszSMh4RaZksXK5MJlztA9DP2pdIvOzgRzuiMhpscvCSL24xAw0JU81vOBPH-JtQi6Zqug7Px_DfNjw0IYj8S2KPrJseCKSmiNPfT7K0kvJj0ZoDIvHOE96nX3Nb2f-Ud3WouMD2O6oWQeHsPuhx07x5AcUKir6cPTYK8QOtaOr3QzfIVjccaEdo4IOfP8bZ1g-1lN0qGnR0qEIKK5uUkWniGf1erZslGQ0XYiEsqH3EXS8nuDZnrqr-5kaZx2CCr9C1uQ9sxu2iH3YjiLS3UoUloGOjzzro5FlRA7frtdY60o0rCHAEmnDtK1sF_41vvep1vwWBVvHkcKRPJ7Fk4UuM_Liyby-aHw2XzRtuNP5zyfFxQ4V8MMcoxha1GJc-OJhqxR7keks5fEpeVZPGJTSp2EsCS4fYdmAhyo6AUBXUydPePvoccMsUO3rWhyoit01iy6nfAxnLWlmrS0m-qjFvmxYKcRFyX4BbIlyxQYRPOZtdPn0hHPtio7cqOqHdbTsY23CScYmBmhnADC5AbHzjacnI09d-icitFGahB2oFQJkAlIBAtyYsvtsju6uo2h8Rg8s2aTZyGisMH04wTfU5zgcOteLV5nDtaYW9OgGurWSv651hcLHLgnFX_AsLmjuGz0a7FcOfqcWVMwlCr8r3YHUmtBznFwmQ_OPpiIPLL0liWcTLdP586EBbHfbhIDT1_BUEPDeHxaVTs6eVU_5cRv24llWLcOciodRse663OL19ruI0jrQqjoCaSoFgCNzAnEwvv1ckDplbspSljYjQx8na8VwBuP3-qU2PYXJ2TV5Sb_PslvJ4nw2KuJDMB31BVLfMtbyYiCuu611AvbQSSzXf7no17P1_iLN6allOd2jc875eA1Gh5bQsYOkc--yWtIoe_6eko5IxQcDk071HevU4uqhi0hJRAG4_HRru6eywYpHImz4iV7z2qClrPyPozY9fZrKf_cRP6weeKOPMEb7y8242QADUCln9TD_UiwyfcIfDAwbVVtQSSA5QcErZU5VaeN_Kvdo9Phh6576vzJuT4IkCm9k7vzQHMaHTjBRHOowkwCHl7xlsieFrRwoNS6lt7nnz__MQnRLMqAI5-GMoTnjKBtoHUilLhzMCINg3RHsBs1xThD8RtV1B7hI-m6eEQEo8EGOxiuttASe6biMF5sjaXHflyHaxWqPv4IGmQDGdUoPmYQOD-pyOUWG__6xiUkTXbruDVTnC1weNxRm4dfP4vrdloJQAyjdS3Z0vYuKm_p9oZc3l4LUzZTgrSQyaDpnsQNsfleDoZVqgedHXtGszWUUWZGNMotOCXmw7wLMy7oi3DB3jsWXXBZEdtsxf9Ze3TfydMIQPZQiurOHhxVLl_5bVJpccjilUCocYgGxhl6ghlkaeZaZFF_umC15gVSieLn5Nmluw7iBjX6ZbaUdCld7OzlVEPYtXg6x2w9dARTO1UMKwMIy7CtXSWopTVW5ppiT1CctC1NFzHCymSD4ptAPhnb2Jnni9CV8KB6tpzGIZjtDe7fj4GR0uDW8yZ3zLeDUr6QfrtnBXwwKrjgavS2FnYhzzz1rb1hfxm_2bRzp1ZcSGQkPpUeSlO2DqvChlXzvduyxrx1Dy0kz5YhVp-Rh8isi4UsxTt5J4hOf3Rhtkk7QgD6htlA8uhIl2Nsa3XeVN22W52IhtmAfWFNac5qk3H3zhXS91gpxF5upE9SJQOXYftNeUyTH4MazSqlV8Bs9OXuxMFO1hq2vkxZ205DtAeVNlVOOu4tuwFGA_5Hy5suip4NBPE4PKyjZCEn9nx95KFoDP2z6hU6rxYxqVA9rsDWM9w0CVDVGLM7im6RD-n57wkvGErxM5VOJ_Y4fDamzk5t3tkoX_aZxoa-DvXvfU9skveuM6lFaJ74bIbIoClq64meFCLWoKkySUWDqA0_EdqS4xWlZItH6ePLMh0ZTjhYnLTaI8ra7LTxpXJxPnjMEflQlZypAvdcvisKUTyHQO7kufaHTBds9aF6ze2w0uYz7N5kdMcOHGz8yurwhH72thnZPLV0FQ0XSVUWn984Adftx_Z72EojvueP10W-u0e1TYAHEqX79DoE_ON9kjsfYnzsNKBJ5TYPYZsYwUMq0M3s_rhCigh20l4LTYrhZ1_MWaFICxq_7JIy3nRN253-1UJQhBB_0000)](https://editor.plantuml.com/uml/lHjTKoEvyNshzZ-e_70nMUFQIzu8eGuCoxgo71GskvMdb1WBCzdno3SpXlLH-z_JBSr7jpxiONC93s1BtUfFjRfRufUoaaMrMMLlt_n9pBwckLZBGgvKfOhoxPiI1intaMIoB4-hgaZlDvMQ9ZgVfRyhyTw3EPF9rsMXD_bYeZDTY4_fyh4wopQAGfQmlDBLTgqcGVX_gYpJpnGZpG7bGIOg27zPA9LJy3L0oMKKU4jXLRvPXG5rjb3vhLuqxA2E3WyFnRbwI7CbRYoHKZm0niKtO32N6KokLGvuIGep0FtsJSsC6Cos8FXg85xUlX7mO_GbJfCgrJaTcPwdvLgNyZvJT7YcYbHcwUyutbf271_BvlF9oTittproOv0eKuP8wP2UNe3egf2LBlPXZYzL3lGJPuN1UMhcPR4zdlz3RKTY_bc2iKy6GfQYcsGu3LcGutXk8SaG0_qeoyUPgXgm-Yi3-PIMxJn-vgHkbLnSvzcMGh51BlXTdbRRYtm9vdNDj5vdQI9TMqqaiir7yYTP-YENu5vhEdm9gDVt_mO5qz4hdUipzIYVKhqfwCGj0CuBcPVWRIlc5qcHhYjKdR_QR5sabV97Rj552yK811V_30lguY_JlF97_qg7liYq-g2BcOBGi2YZMXrVFAcyilxbkkEPB9MPFfSLKod1iSDjD13J6ss-NE2lZp9VP8rG0PXlQRK7P08s_Xg3cIdS3R7PSrNwyvR_XdudMqmxDrfdxXQpLiMPne6xaVXAimGsx9NCOM85P9r5oMpXZTi138lYavOB2wiNAavidcPg9TUkGUjXTmZj75_hRkgwnQQgd02v0Yo-mldbUFuCXq2QBntqysip0O4IZhegJJ1Aqg3wsmPYDTiY5nv_ywkRGhl2gM_LbIeVOURkycvg49e3OcOeYOygMwj2J13UEmpifAio2F9FQQ8-wGIZCc6w0SEV5pluNRo3gB1CyJG5rnZEHMcn3uAWboQE3W_4aPW70UxoOiTgt-qVXy7n-8I5UIfBBU5-8O0p6qSDSpIa-e0OPuOFcPQLU8QFYE26dWRndVWW5sgQ3qa05ibo92nwbQvK-rcBlukV7mvSvEjDrHVx_GDNL2EzqP3rGUOZqoPqk8xHJRI-GSBCHAzM466e9jz1yg4II02C-kvDM22C4EIxzKAQauAZpo7erJfTgAoISv2D2-EjD9V5KWMM-gABR76ZonGz0hYfJmhn8W2yMSs2JilJB7rIDSiX23npj-SIT_iGQ0kvWWoj6f6JKvHmIhekwZ5xaQlLzeD4JhQStKcX9EYjuHAohf43heiKmX8grWnJv5k8zn0gXcO2uYNyvepmbT0H1ZW2YU7Op0mY_69azrKxXI1f4i0-_78-IrKD7TPQ6iWMMsAdnI6-pYh86DNIqQGz72tGa6PuPkaJaO3Jfk1z2ZYUwsS8YnN-TXJRSESJYsel1ck8b6Fp9wPeCnbqZ7cnhNrZ8ei57jeWGR4O2QRwBZiIoo95lny9lQaQ0Vbsv4v3TU38JfaoeY3vSSF780hcI4nYpKGNokmQSp8f9-1q4w60Op5ukC5PCpvaZeO9Lc3OPaHimWzDiyrzZ19VpSIm066ojBSuIcXMd-YyAh2gAPmuy2ZpPSrYBN6kdiq7npgXfSmMCeCHFskHwjFJ8Ruf2anm3TcZLkD-FBpEBMN3x090RUruR35E8emsD2cNRXrIfu82SXge39mgX79xAFw5nt6BS2HQEYoThBcWQnyUddZ0W3z8p3uhsHxjE6FGwMgTcNIszEKR1MqpYXvwFRHEX61iT9f4_bTcDCJ_5UJG5emopjoy9j51chK3WeJWRZAvDN6lvYIkIGOPPQOsS59hfjcHHztkf0gaQvhZELDFAkFRXT55bddw600Q_3mG6aBMuFru31-dX0j7qzxQWxlfUFQedp5IGkwqKo4j7DT680YXGW2mSzX9zurdnksICK4vnA1Uf1oJ5GtRcpBCCxRgliWdjPFdxxmICLMpQ4Y98TlPvK6mGzM2csgpnMM5oaSbi-gnTzo89NDkpamNZQdHtM9tqrUmWJQO9TWl7D8qqemSU7koMph6J0YAbfIiiCIqvJMXLjby1toYQOJPR8eA13LHdHPDyriy67XsQDFw0YSm7J5zCBf8BInZ3VVWuQ4sU_3F0pVjydSYG-vZjNhhzZGSZoNxJH84n7r33froSmysJ-_TQCAH87CszSMh4RaZksXK5MJlztA9DP2pdIvOzgRzuiMhpscvCSL24xAw0JU81vOBPH-JtQi6Zqug7Px_DfNjw0IYj8S2KPrJseCKSmiNPfT7K0kvJj0ZoDIvHOE96nX3Nb2f-Ud3WouMD2O6oWQeHsPuhx07x5AcUKir6cPTYK8QOtaOr3QzfIVjccaEdo4IOfP8bZ1g-1lN0qGnR0qEIKK5uUkWniGf1erZslGQ0XYiEsqH3EXS8nuDZnrqr-5kaZx2CCr9C1uQ9sxu2iH3YjiLS3UoUloGOjzzro5FlRA7frtdY60o0rCHAEmnDtK1sF_41vvep1vwWBVvHkcKRPJ7Fk4UuM_Liyby-aHw2XzRtuNP5zyfFxQ4V8MMcoxha1GJc-OJhqxR7keks5fEpeVZPGJTSp2EsCS4fYdmAhyo6AUBXUydPePvoccMsUO3rWhyoit01iy6nfAxnLWlmrS0m-qjFvmxYKcRFyX4BbIlyxQYRPOZtdPn0hHPtio7cqOqHdbTsY23CScYmBmhnADC5AbHzjacnI09d-icitFGahB2oFQJkAlIBAtyYsvtsju6uo2h8Rg8s2aTZyGisMH04wTfU5zgcOteLV5nDtaYW9OgGurWSv651hcLHLgnFX_AsLmjuGz0a7FcOfqcWVMwlCr8r3YHUmtBznFwmQ_OPpiIPLL0liWcTLdP586EBbHfbhIDT1_BUEPDeHxaVTs6eVU_5cRv24llWLcOciodRse663OL19ruI0jrQqjoCaSoFgCNzAnEwvv1ckDplbspSljYjQx8na8VwBuP3-qU2PYXJ2TV5Sb_PslvJ4nw2KuJDMB31BVLfMtbyYiCuu611AvbQSSzXf7no17P1_iLN6allOd2jc875eA1Gh5bQsYOkc--yWtIoe_6eko5IxQcDk071HevU4uqhi0hJRAG4_HRru6eywYpHImz4iV7z2qClrPyPozY9fZrKf_cRP6weeKOPMEb7y8242QADUCln9TD_UiwyfcIfDAwbVVtQSSA5QcErZU5VaeN_Kvdo9Phh6576vzJuT4IkCm9k7vzQHMaHTjBRHOowkwCHl7xlsieFrRwoNS6lt7nnz__MQnRLMqAI5-GMoTnjKBtoHUilLhzMCINg3RHsBs1xThD8RtV1B7hI-m6eEQEo8EGOxiuttASe6biMF5sjaXHflyHaxWqPv4IGmQDGdUoPmYQOD-pyOUWG__6xiUkTXbruDVTnC1weNxRm4dfP4vrdloJQAyjdS3Z0vYuKm_p9oZc3l4LUzZTgrSQyaDpnsQNsfleDoZVqgedHXtGszWUUWZGNMotOCXmw7wLMy7oi3DB3jsWXXBZEdtsxf9Ze3TfydMIQPZQiurOHhxVLl_5bVJpccjilUCocYgGxhl6ghlkaeZaZFF_umC15gVSieLn5Nmluw7iBjX6ZbaUdCld7OzlVEPYtXg6x2w9dARTO1UMKwMIy7CtXSWopTVW5ppiT1CctC1NFzHCymSD4ptAPhnb2Jnni9CV8KB6tpzGIZjtDe7fj4GR0uDW8yZ3zLeDUr6QfrtnBXwwKrjgavS2FnYhzzz1rb1hfxm_2bRzp1ZcSGQkPpUeSlO2DqvChlXzvduyxrx1Dy0kz5YhVp-Rh8isi4UsxTt5J4hOf3Rhtkk7QgD6htlA8uhIl2Nsa3XeVN22W52IhtmAfWFNac5qk3H3zhXS91gpxF5upE9SJQOXYftNeUyTH4MazSqlV8Bs9OXuxMFO1hq2vkxZ205DtAeVNlVOOu4tuwFGA_5Hy5suip4NBPE4PKyjZCEn9nx95KFoDP2z6hU6rxYxqVA9rsDWM9w0CVDVGLM7im6RD-n57wkvGErxM5VOJ_Y4fDamzk5t3tkoX_aZxoa-DvXvfU9skveuM6lFaJ74bIbIoClq64meFCLWoKkySUWDqA0_EdqS4xWlZItH6ePLMh0ZTjhYnLTaI8ra7LTxpXJxPnjMEflQlZypAvdcvisKUTyHQO7kufaHTBds9aF6ze2w0uYz7N5kdMcOHGz8yurwhH72thnZPLV0FQ0XSVUWn984Adftx_Z72EojvueP10W-u0e1TYAHEqX79DoE_ON9kjsfYnzsNKBJ5TYPYZsYwUMq0M3s_rhCigh20l4LTYrhZ1_MWaFICxq_7JIy3nRN253-1UJQhBB_0000)

## GDD

Link to our GDD:

https://docs.google.com/document/d/1sI9DLnQetL0x-KTbqv_ieDqXfi3fZ07AztViNrDKycw/edit?usp=drivesdk
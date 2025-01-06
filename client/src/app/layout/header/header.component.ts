import { Component } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { MatButtonModule} from '@angular/material/button';
import { MatBadge} from '@angular/material/badge';


@Component({
    selector: 'app-header',
    standalone: true,
    imports: [MatIcon, MatButtonModule, MatBadge],
    templateUrl: './header.component.html',
    styleUrl: './header.component.scss'
})
export class HeaderComponent {}

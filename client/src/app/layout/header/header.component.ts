import { Component, inject } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { MatButtonModule} from '@angular/material/button';
import { MatBadge} from '@angular/material/badge';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { BusyService } from '../../Core/services/busy.service';
import{ MatProgressBarModule} from '@angular/material/progress-bar'


@Component({
    selector: 'app-header',
    standalone: true,
    imports: [MatIcon, MatButtonModule, MatBadge, RouterLink, RouterLinkActive,
MatProgressBarModule    ],
    templateUrl: './header.component.html',
    styleUrl: './header.component.scss'
})
export class HeaderComponent {
busyService= inject(BusyService);

}

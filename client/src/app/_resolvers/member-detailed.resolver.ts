import { ActivatedRouteSnapshot, ResolveFn, RouterStateSnapshot } from '@angular/router';
import { Member } from '../_models/member';
import { inject } from '@angular/core';
import { MemberService } from '../_services/member.service';

export const memberDetailedResolver: ResolveFn<Member> = (route:ActivatedRouteSnapshot, state : RouterStateSnapshot ) => {
  const memberService =  inject(MemberService); 

  return memberService.getMember(route.paramMap.get('username')!)
};
